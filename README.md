<div align="center">
  <img src="https://github.com/domoinc/domo-node-sdk/blob/master/domo.png?raw=true" width="400" height="400"/>
</div>

# AspNetCore - Private Embed for cards with Programmatic Filtering Example Code
[![License](https://img.shields.io/badge/license-MIT-blue.svg?style=flat)](http://www.opensource.org/licenses/MIT)

### About

* Example AspNetCore server demonstrating private embed for cards with programmatic filtering

### Setup

1. Download the source code.
2. Open up the solution file in Visual Studio named ProgrammaticFiltering.sln.
3. Edit the ProgrammaticFilteringService.cs file and replace the following items:
  * CLIENT_ID with your client id
  * CLIENT_SECRET with your client secret
  * EMBED_ID with the embedded id of your card e.g. "ej73t"
  * Modify the filter each user receives by replacing [] with any filtering you want to do such as "[{"column": "Region", "operator": "IN", "values": ["West"]}]".  
  The complete list of available operators for use in filters are as follows: "IN", "NOT_IN", "EQUALS", "NOT_EQUALS", "GREATER_THAN", "GREAT_THAN_EQUALS_TO", "LESS_THAN", "LESS_THAN_EQUALS_TO".  
  Specify different filters for each user to see the ability of programmatic filtering in action.  
5. Start the IIS Express server in Visual Studio and go to the page that loads in your browser.
6. The possible logins are mike@test.com, susan@test.com, and tom@test.com. The password for all the users is "test".

The EMBED_ID represents the public or private identifier for the card. 
The CLIENT_ID and CLIENT_SECRET is used to create the access token which will be used to then create an embed token for use with the private embed.
For more information about creating the CLIENT_ID and CLIENT_SECRET see https://developer.domo.com/docs/authentication/overview-4.
   
