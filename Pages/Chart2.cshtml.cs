using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProgrammaticFiltering.Authorization;
using ProgrammaticFiltering.Data;
using ProgrammaticFiltering.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ProgrammaticFiltering.Pages
{
    public class Chart2Model : DI_BasePageModel
    {
        public string url;
        public string embedToken;

        /// <summary>
        /// Set when token creation fails. The view renders this instead of submitting the
        /// form, so configuration problems are visible rather than showing a blank iframe.
        /// </summary>
        public string error;

        public Chart2Model(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager,
            IProgrammaticFilteringService programmaticFilteringService)
            : base(context, authorizationService, userManager, programmaticFilteringService)
        {
        }

        public async Task OnGetAsync()
        {
            var username = UserManager.GetUserName(User);
            ProgrammaticFilter programmaticFilter = ProgrammaticFilteringService.getProgrammaticFilter(username);

            url = Constants.EmbedUrl + programmaticFilter.EmbedId;
            var domoHttpClient = new DomoHttpClient();

            try
            {
                var accessToken = await domoHttpClient.GetAccessTokenAsync(programmaticFilter.ClientId, programmaticFilter.ClientSecret);
                embedToken = await domoHttpClient.GetEmbedToken(accessToken, programmaticFilter.EmbedId, programmaticFilter.Filter);
            }
            catch (DomoEmbedException e)
            {
                error = e.Message;
            }
        }
    }
}
