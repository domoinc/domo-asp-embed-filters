namespace ProgrammaticFiltering
{
    /// <summary>
    /// Domo endpoints used by this sample.
    ///
    /// This sample embeds <b>dashboards</b> (a dashboard and a page are the same
    /// surface, hence one set of constants). The card and App Studio constants below
    /// are provided so the same flow can be pointed at those surfaces instead — see
    /// the "Embedding something other than a dashboard" section of the README.
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
        // Render URLs -- the form in Chart1/Chart2 POSTs the embed token here
        // ------------------------------------------------------------------

        // Every surface except card embed v1 is served by a single endpoint that
        // resolves the entity type from the embed id, so dashboards, cards (v2)
        // and App Studio apps all share one render URL. The /embed/pages/,
        // /embed/cards/, /embed/dashboards/ and /embed/app-studio/ paths are
        // aliases of this same endpoint and are what Domo's own embed dialog
        // currently hands out; they continue to work if you prefer to match it.
        public static readonly string EmbedUrlEntities = EmbedHost + "/embed/entities/";

        // Card embed v1 -- legacy, and the one surface that genuinely needs its own
        // path. It is a different renderer, handling chart and DomoApp cards only,
        // with a JS API limited to /v1/onDrill, /v1/onFiltersChange,
        // /v1/onFrameSizeChange and /v1/filters/apply. It cannot be served from
        // /embed/entities/, which always renders the current card experience.
        public static readonly string EmbedUrlCardV1 = EmbedHost + "/cards/";

        // ------------------------------------------------------------------
        // Active configuration used by Chart1 / Chart2.
        //
        // This sample embeds dashboards. EmbedUrlEntities already serves cards
        // (v2) and App Studio apps too, so to embed one of those you only need to
        // switch the token endpoint to match the published docs for that surface:
        //
        //     EmbedTokenUrl = EmbedTokenUrlCard;
        //
        // Card embed v1 is the exception -- it needs its own render URL:
        //
        //     EmbedTokenUrl = EmbedTokenUrlCard;
        //     EmbedUrl      = EmbedUrlCardV1;
        //
        // Mismatching these (for example a dashboard token endpoint with a card
        // render URL, or an EMBED_ID that names a card while these point at a
        // dashboard) is what produces a 404 inside the iframe.
        // ------------------------------------------------------------------
        public static readonly string EmbedTokenUrl = EmbedTokenUrlDashboard;
        public static readonly string EmbedUrl = EmbedUrlEntities;
    }
}
