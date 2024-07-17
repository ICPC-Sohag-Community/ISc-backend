using ISc.Domain.Comman.Constant;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace ISc.Presentation.Middlerware
{
    public class MobileAuthorizeFilter : Attribute, IAsyncAuthorizationFilter
    {
        private UserManager<Account>? _userManager;

        public MobileAuthorizeFilter()
        {
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            _userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<Account>>();

            var isAuthneticated = context.HttpContext.User.Identity!.IsAuthenticated;
            var user = await _userManager.GetUserAsync(context.HttpContext.User);

            if (user == null || isAuthneticated == false)
            {
                var response = await Response.FailureAsync("Unauthorized.", HttpStatusCode.Unauthorized);
                context.Result = new OkObjectResult(response);

                return;
            }

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Count == 1 && roles.First() == Roles.Trainee)
            {
                var response = await Response.FailureAsync("Forbidden.", HttpStatusCode.Forbidden);
                context.Result = new OkObjectResult(response);
            }
        }
    }
}
