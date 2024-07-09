using ISc.Application.Interfaces;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace ISc.Application.Features.Authentication.Login
{
    public class LoginQuery : IRequest<Response>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
    public class LoginQueryHandler : IRequestHandler<LoginQuery, Response>
    {
        private readonly UserManager<Account> _userManager;
        private readonly SignInManager<Account> _signInManager;
        private readonly IAuthServices _authServices;

        public LoginQueryHandler(
            UserManager<Account> userManager,
            IAuthServices authServices,
            SignInManager<Account> signInManager)
        {
            _userManager = userManager;
            _authServices = authServices;
            _signInManager = signInManager;
        }

        public async Task<Response> Handle(LoginQuery query, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(query.UserName);

            if (user is null)
            {
                return await Response.FailureAsync("UserName or password is wrong");
            }

            var signInResult = await _signInManager.PasswordSignInAsync(user, query.Password, query.RememberMe, true);

            if (!signInResult.Succeeded && signInResult.IsLockedOut)
            {
                return await Response.FailureAsync("Your account locked for a while,Please try again later..", HttpStatusCode.Forbidden);
            }
            else if (!signInResult.Succeeded)
            {
                return await Response.FailureAsync("UserName or password is wrong");
            }

            user.LastLoginDate = DateTime.Now;
            await _userManager.UpdateAsync(user);

            var roles = await _userManager.GetRolesAsync(user);

            var response = user.Adapt<LoginQueryResponse>();

            response.Token = _authServices.GenerateToken(user, roles, query.RememberMe);
            response.Roles = roles;

            return await Response.SuccessAsync(response, "Success");
        }
    }
}
