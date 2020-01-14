using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProgrammaticFiltering.Models;

namespace ProgrammaticFiltering
{
    public class ProgrammaticFilteringService : IProgrammaticFilteringService
    {
        private Dictionary<string, ProgrammaticFilter> filters = new Dictionary<string, ProgrammaticFilter>();

        public ProgrammaticFilteringService() {
            filters.Add("mike@test.com",
                new ProgrammaticFilter
                    {
                    ClientId = "CLIENT_ID",
                    ClientSecret = "CLIENT_SECRET",
                    Filter = "[]",
                    EmbedId = "EMBED_ID"
                });
            filters.Add("susan@test.com",
                    new ProgrammaticFilter
                    {
                        ClientId = "CLIENT_ID",
                        ClientSecret = "CLIENT_SECRET",
                        Filter = "[]",
                        EmbedId = "EMBED_ID",
                    });
            filters.Add("tom@test.com",
                    new ProgrammaticFilter
                    {
                        ClientId = "CLIENT_ID",
                        ClientSecret = "CLIENT_SECRET",
                        Filter = "[]",
                        EmbedId = "EMBED_ID"
                    });
        }

        public ProgrammaticFilter getProgrammaticFilter(string username)
        {
            if (!filters.TryGetValue(username, out ProgrammaticFilter filter))
            {
                return new ProgrammaticFilter();
            }
            return filters[username];
        }
    }
}