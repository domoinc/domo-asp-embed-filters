namespace ProgrammaticFiltering
{
    public class Constants
    {
        private static readonly string ApiHost = "https://api.domo.com";
        private static readonly string EmbedHost = "https://public.domo.com";
        public static readonly string AccessTokenUrl = ApiHost + "/oauth/token?grant_type=client_credentials&scope=data%20audit%20user%20dashboard";
        public static readonly string EmbedTokenUrl = ApiHost + "/v1/stories/embed/auth";
        public static readonly string EmbedUrl = EmbedHost + "/embed/pages/";
    }
}
