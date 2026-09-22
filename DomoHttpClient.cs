using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammaticFiltering
{
    /// <summary>
    /// Thrown when Domo rejects an access-token or embed-token request, or returns a
    /// token that grants nothing. The sample pages render the message so the failure
    /// is visible in the browser instead of showing an empty iframe.
    /// </summary>
    public class DomoEmbedException : Exception
    {
        public DomoEmbedException(string message) : base(message) { }
    }

    public class DomoHttpClient
    {
        private HttpClient _client = new HttpClient();

        public async Task<string> GetAccessTokenAsync(string clientId, string clientSecret)
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}")));

            var httpResponseMessage = await _client.GetAsync(Constants.AccessTokenUrl);
            var body = await httpResponseMessage.Content.ReadAsStringAsync();
            _client.DefaultRequestHeaders.Accept.Clear();

            System.Diagnostics.Debug.WriteLine($"Access token response status = {(int)httpResponseMessage.StatusCode} {httpResponseMessage.StatusCode}");

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new DomoEmbedException(
                    $"Access-token request failed: {(int)httpResponseMessage.StatusCode} {httpResponseMessage.StatusCode}. " +
                    "Check the CLIENT_ID and CLIENT_SECRET values in ProgrammaticFilteringService.cs. " +
                    $"Response: {body}");
            }

            var accessToken = (string)ParseJsonOrThrow(body, "access-token")["access_token"];

            if (string.IsNullOrEmpty(accessToken))
            {
                throw new DomoEmbedException($"Access-token response did not contain an access_token. Response: {body}");
            }

            return accessToken;
        }

        public async Task<string> GetEmbedToken(string accessToken, string urn, string filter)
        {
            var data = new StringContent(
                $"{{ \"sessionLength\":1440, \"authorizations\":[{{ \"token\":\"{urn}\", \"permissions\":[\"READ\",\"FILTER\",\"EXPORT\"], \"filters\": {filter} }}]}}",
                Encoding.UTF8, "application/json");

            System.Diagnostics.Debug.WriteLine($"POST data = {await data.ReadAsStringAsync()}");

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var httpResponseMessage = await _client.PostAsync(Constants.EmbedTokenUrl, data);
            var body = await httpResponseMessage.Content.ReadAsStringAsync();
            _client.DefaultRequestHeaders.Accept.Clear();

            System.Diagnostics.Debug.WriteLine($"Embed token response status = {(int)httpResponseMessage.StatusCode} {httpResponseMessage.StatusCode}");
            System.Diagnostics.Debug.WriteLine($"Embed token response content = {body}");

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new DomoEmbedException(
                    $"Embed-token request to {Constants.EmbedTokenUrl} failed: " +
                    $"{(int)httpResponseMessage.StatusCode} {httpResponseMessage.StatusCode}. " +
                    "A 404 here usually means EmbedTokenUrl does not match the kind of content EMBED_ID points at — " +
                    "this sample is configured for dashboards, so embedding a card requires the card constants in Constants.cs. " +
                    $"Response: {body}");
            }

            var json = ParseJsonOrThrow(body, "embed-token");
            var embedToken = (string)json["authentication"];

            if (string.IsNullOrEmpty(embedToken))
            {
                throw new DomoEmbedException($"Embed-token response did not contain an authentication value. Response: {body}");
            }

            // Domo reports any embed ids it refused to authorize. Newer versions reject the
            // whole request instead, so handle both and always report the reason.
            if (json["forbiddenCardTokens"] is JArray forbidden && forbidden.Count > 0)
            {
                throw new DomoEmbedException(
                    $"Domo refused to authorize the following embed id(s): {forbidden.ToString(Formatting.None)}. " +
                    "The user associated with CLIENT_ID and CLIENT_SECRET does not have access to that content.");
            }

            EnsureEmbedClaimIsNotEmpty(embedToken, body);

            System.Diagnostics.Debug.WriteLine($"embedToken: {embedToken}");
            return embedToken;
        }

        private static JObject ParseJsonOrThrow(string body, string requestName)
        {
            try
            {
                return JObject.Parse(body);
            }
            catch (JsonException)
            {
                throw new DomoEmbedException($"Could not parse the {requestName} response as JSON. Response: {body}");
            }
        }

        /// <summary>
        /// The "emb" claim lists the embed ids a token actually grants access to. An empty
        /// array means the token is valid but authorizes nothing, which renders as an empty
        /// iframe with no other error — almost always because the user behind CLIENT_ID and
        /// CLIENT_SECRET cannot see the dashboard or card named by EMBED_ID.
        /// </summary>
        private static void EnsureEmbedClaimIsNotEmpty(string jwt, string responseBody)
        {
            var segments = jwt.Split('.');
            if (segments.Length < 2)
            {
                return;
            }

            // Base64url -> base64, then restore the padding the JWT encoding strips.
            var payload = segments[1].Replace('-', '+').Replace('_', '/');
            payload = payload.PadRight(payload.Length + ((4 - (payload.Length % 4)) % 4), '=');

            JObject claims;
            try
            {
                claims = JObject.Parse(Encoding.UTF8.GetString(Convert.FromBase64String(payload)));
            }
            catch (Exception)
            {
                // Decoding only improves the error message, so a failure here must not
                // break an otherwise valid token.
                return;
            }

            if (claims["emb"] is JArray emb && emb.Count == 0)
            {
                throw new DomoEmbedException(
                    "The embed token's \"emb\" claim is empty, so it grants access to nothing and the " +
                    "iframe would render blank. This usually means the user associated with CLIENT_ID " +
                    "and CLIENT_SECRET does not have access to the dashboard or card named by EMBED_ID. " +
                    $"Response: {responseBody}");
            }
        }
    }
}
