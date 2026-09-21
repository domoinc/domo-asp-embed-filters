namespace ProgrammaticFiltering
{
    /// <summary>
    /// Domo endpoints used by this sample.
    ///
    /// This sample embeds <b>dashboards</b> (also called pages). The card constants
    /// below are provided so the same flow can be pointed at a card embed instead —
    /// see the "Embedding a card instead of a dashboard" section of the README.
    /// </summary>
    public class Constants
    {
        private static readonly string ApiHost = "https://api.domo.com";
        private static readonly string EmbedHost = "https://public.domo.com";

        public static readonly string AccessTokenUrl = ApiHost + "/oauth/token?grant_type=client_credentials&scope=data%20audit%20user%20dashboard";

        // ------------------------------------------------------------------
        // Embed-token endpoints
        //
        // Domo resolves the entity type (dashboard vs card) from the embed id
        // itself, so either endpoint will mint a usable token for either kind of
        // content. They are kept separate here to match the published docs for
        // each surface.
        // ------------------------------------------------------------------

        // Dashboard / page. "/v1/dashboards/embed/auth" is an alias for this same
        // endpoint with clearer naming; "stories" is legacy terminology.
        public static readonly string EmbedTokenUrlDashboard = ApiHost + "/v1/stories/embed/auth";

        // Cards. Used for BOTH card embed v1 and v2 — the version is determined by
        // the render URL below, not by which token endpoint you call.
        public static readonly string EmbedTokenUrlCard = ApiHost + "/v1/cards/embed/auth";

        // ------------------------------------------------------------------
        // Render URLs — the form in Chart1/Chart2 POSTs the embed token here
        // ------------------------------------------------------------------

        // Dashboard / page embed. There is no v1/v2 split for dashboards; this is
        // already served by the current-generation embed backend.
        public static readonly string EmbedUrlDashboard = EmbedHost + "/embed/pages/";

        // Card embed v2 — the default choice for cards. Served by the same backend
        // as dashboard embed, so it supports more card types (Notebook/Text in
        // addition to chart and DomoApp) and the full JS API, including
        // /v1/onAppData, /v1/onAppReady and /v1/appData/apply.
        public static readonly string EmbedUrlCardV2 = EmbedHost + "/embed/cards/";

        // Card embed v1 — legacy. Served by the older renderer: chart and DomoApp
        // cards only, and the JS API is limited to /v1/onDrill,
        // /v1/onFiltersChange, /v1/onFrameSizeChange and /v1/filters/apply.
        // Prefer EmbedUrlCardV2 for new integrations.
        public static readonly string EmbedUrlCardV1 = EmbedHost + "/cards/";

        // ------------------------------------------------------------------
        // Active configuration used by Chart1 / Chart2.
        //
        // This sample embeds dashboards. To embed a card instead, change BOTH
        // lines together — use EmbedUrlCardV2 unless you specifically need v1:
        //
        //     EmbedTokenUrl = EmbedTokenUrlCard;
        //     EmbedUrl      = EmbedUrlCardV2;
        //
        // Mismatching these (for example a dashboard token endpoint with a card
        // render URL, or an EMBED_ID that names a card while these point at a
        // dashboard) is what produces a 404 inside the iframe.
        // ------------------------------------------------------------------
        public static readonly string EmbedTokenUrl = EmbedTokenUrlDashboard;
        public static readonly string EmbedUrl = EmbedUrlDashboard;
    }
}
