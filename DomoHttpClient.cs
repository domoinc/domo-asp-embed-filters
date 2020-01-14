using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammaticFiltering
{
    public class DomoHttpClient
    {
        private HttpClient _client = new HttpClient();

        public async Task<string> GetAccessTokenAsync(string clientId, string clientSecret)
        {
            var httpResponseMessage = new HttpResponseMessage();
            var accessToken = "";

            try
            {
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}")));
                httpResponseMessage = await _client.GetAsync(Constants.AccessTokenUrl);
                System.Diagnostics.Debug.WriteLine($"Embed Token response status = {httpResponseMessage.StatusCode}");
                System.Diagnostics.Debug.WriteLine($"Embed Token response content = {httpResponseMessage.Content}");
                var accessTokenJson = await httpResponseMessage.Content.ReadAsStringAsync();
                _client.DefaultRequestHeaders.Accept.Clear();

                dynamic deserializedAccessToken = JsonConvert.DeserializeObject(accessTokenJson);
                accessToken = (string)deserializedAccessToken.access_token;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Embed Token response received an exception: {e.Message}");
                _client.DefaultRequestHeaders.Accept.Clear();
            }

            return accessToken;
        }

        public async Task<string> GetEmbedToken(string accessToken, string urn, string filter)
        {
            var httpResponseMessage = new HttpResponseMessage();
            var embedToken = "";
            var data = new StringContent($"{{ \"sessionLength\":1440, \"authorizations\":[{{ \"token\":\"{urn}\", \"permissions\":[\"READ\",\"FILTER\",\"EXPORT\"], \"filters\": {filter} }}]}}", Encoding.UTF8, "application/json");

            System.Diagnostics.Debug.WriteLine($"POST data = {await data.ReadAsStringAsync()}");
            try
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                httpResponseMessage = await _client.PostAsync(Constants.EmbedTokenUrl, data);
                int statusCode = (int)httpResponseMessage.StatusCode;
                System.Diagnostics.Debug.WriteLine($"Embed Token response status: {statusCode} {httpResponseMessage.StatusCode}");
                var content = await httpResponseMessage.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"Embed Token response content: {content}");
                if (statusCode == 200)
                {
                    dynamic deserializedEmbedToken = JsonConvert.DeserializeObject(content);
                    embedToken = deserializedEmbedToken.authentication;
                    System.Diagnostics.Debug.WriteLine($"embedToken: {embedToken}");
                }
                _client.DefaultRequestHeaders.Accept.Clear();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Embed Token response received an exception: {e.Message}");
                _client.DefaultRequestHeaders.Accept.Clear();
            }

            return embedToken;
        }
    }
}
