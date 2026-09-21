<div align="center">
  <img src="https://github.com/domoinc/domo-node-sdk/blob/master/domo.png?raw=true" width="400" height="400"/>
</div>

# AspNetCore - Private Embed for dashboards with Programmatic Filtering Example Code
[![License](https://img.shields.io/badge/license-MIT-blue.svg?style=flat)](http://www.opensource.org/licenses/MIT)

### About

* Example AspNetCore server demonstrating private embed for dashboards with programmatic filtering

This sample is configured for **dashboard** (page) embed out of the box. It can also be
pointed at a **card** embed — see [Embedding a card instead of a dashboard](#embedding-a-card-instead-of-a-dashboard).

### Setup

1. Download the source code.
2. Open up the solution file in Visual Studio named ProgrammaticFiltering.sln.
3. Edit the ProgrammaticFilteringService.cs file and replace the following items:
  * CLIENT_ID with your client id
  * CLIENT_SECRET with your client secret
  * EMBED_ID with the embedded id of your dashboard e.g. "ej73t"
  * Modify the filter each user receives by replacing [] with any filtering you want to do such as "[{"column": "Region", "operator": "IN", "values": ["West"]}]".  
  The complete list of available operators for use in filters are as follows: "IN", "NOT_IN", "EQUALS", "NOT_EQUALS", "GREATER_THAN", "GREAT_THAN_EQUALS_TO", "LESS_THAN", "LESS_THAN_EQUALS_TO".  
  Specify different filters for each user to see the ability of programmatic filtering in action.  
5. Start the IIS Express server in Visual Studio and go to the page that loads in your browser.
6. The possible logins are mike@test.com, susan@test.com, and tom@test.com. The password for all the users is "test".

The EMBED_ID represents the public or private identifier for the dashboard. 
The CLIENT_ID and CLIENT_SECRET is used to create the access token which will be used to then create an embed token for use with the private embed.
For more information about creating the CLIENT_ID and CLIENT_SECRET see https://developer.domo.com/docs/authentication/overview-4.

### Embedding a card instead of a dashboard

The endpoints live in `Constants.cs`. Two values decide what gets embedded, and they must
agree with the kind of content `EMBED_ID` points at:

| Content | `EmbedTokenUrl` | `EmbedUrl` |
|---|---|---|
| Dashboard / page (this sample's default) | `EmbedTokenUrlDashboard` | `EmbedUrlDashboard` |
| **Card — v2 (recommended)** | `EmbedTokenUrlCard` | `EmbedUrlCardV2` |
| Card — v1 (legacy) | `EmbedTokenUrlCard` | `EmbedUrlCardV1` |

To embed a card, change both active lines at the bottom of `Constants.cs` together:

```csharp
public static readonly string EmbedTokenUrl = EmbedTokenUrlCard;
public static readonly string EmbedUrl      = EmbedUrlCardV2;
```

A **404 inside the iframe** is the usual symptom of these not matching — for example
leaving the dashboard defaults in place while `EMBED_ID` names a card.

#### Card embed v1 vs v2

`v2` is the current generation and the right default for new integrations. It is served by
the same backend as dashboard embed, which is why it supports more than v1 does.

| | v1 (`/cards/`) | v2 (`/embed/cards/`) |
|---|---|---|
| Card types | Chart and DomoApp only | Chart, DomoApp, plus Notebook/Text |
| JS API events received | `/v1/onDrill`, `/v1/onFiltersChange`, `/v1/onFrameSizeChange` | the same, plus `/v1/onAppData` and `/v1/onAppReady` |
| JS API methods you can call | `/v1/filters/apply` | the same, plus `/v1/appData/apply` |
| Appearance parameters | — | `backgroundColor`, `scaleLineColor`, `textColor` |
| Card image endpoint | `GET /cards/{id}.png` | no drop-in equivalent |

Notes that apply to both versions:

* The **embed-token endpoint does not determine the version.** Domo resolves the content
  type from the embed id itself, so `EmbedTokenUrlCard` is correct for v1 and v2 alike —
  only `EmbedUrl` differs. (`/v1/dashboards/embed/auth` is likewise an alias for
  `/v1/stories/embed/auth`; "stories" is legacy terminology.)
* **Dashboard embed has no v1/v2 split.** `EmbedUrlDashboard` is already current.
* The **JS API silently does nothing unless embed authorized domains are configured** for
  your instance. Add `?debug-js-api` to the embed URL to log why it did not initialise.

### Troubleshooting

Token failures are reported in the iframe rather than rendering blank. If a chart area
shows a red message, it names the failing request, the HTTP status and Domo's response.

* **`Access-token request failed: 401`** — `CLIENT_ID` / `CLIENT_SECRET` in
  `ProgrammaticFilteringService.cs` are wrong, or the client lacks the required scopes.
* **`Embed-token request ... failed: 404`** — `EmbedTokenUrl` / `EmbedUrl` do not match the
  kind of content `EMBED_ID` names. See the table above.
* **`Domo refused to authorize the following embed id(s)`** or **`the "emb" claim is
  empty`** — the token was issued but grants nothing. The user tied to `CLIENT_ID` /
  `CLIENT_SECRET` does not have access to that dashboard or card. Share the content with
  that user, then retry.
