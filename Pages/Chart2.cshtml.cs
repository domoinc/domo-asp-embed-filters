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

            var accessToken = await domoHttpClient.GetAccessTokenAsync(programmaticFilter.ClientId, programmaticFilter.ClientSecret);
            embedToken = await domoHttpClient.GetEmbedToken(accessToken, programmaticFilter.EmbedId, programmaticFilter.Filter);
        }
    }
}
