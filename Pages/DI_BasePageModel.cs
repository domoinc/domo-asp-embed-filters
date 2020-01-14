using ProgrammaticFiltering.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ProgrammaticFiltering.Pages
{
    public class DI_BasePageModel : PageModel
    {
        protected ApplicationDbContext Context { get; }
        protected IAuthorizationService AuthorizationService { get; }
        protected UserManager<IdentityUser> UserManager { get; }
        protected IProgrammaticFilteringService ProgrammaticFilteringService { get; }

        public DI_BasePageModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager,
            IProgrammaticFilteringService programmaticFilteringService) : base()
        {
            Context = context;
            UserManager = userManager;
            AuthorizationService = authorizationService;
            ProgrammaticFilteringService = programmaticFilteringService;
        }
    }
}
