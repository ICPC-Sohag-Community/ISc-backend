using ISc.Application.Features.Authentication.Login;
using ISc.Application.Interfaces;
using ISc.Domain.Comman.Constant;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace ISc.Application.Features.Authentication.MobileLogin
{
    public class MobileLoginQuery : IRequest<Response>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    internal class MobileLoginQueryHandler : IRequestHandler<MobileLoginQuery, Response>
    {
        private readonly UserManager<Account> _userManager;
        private readonly SignInManager<Account> _signInManager;
        private readonly IAuthServices _authServices;

        public MobileLoginQueryHandler(
            UserManager<Account> userManager,
            SignInManager<Account> signInManager,
            IAuthServices authServices)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authServices = authServices;
        }

        public async Task<Response> Handle(MobileLoginQuery query, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(query.UserName);

            if (user is null)
            {
                return await Response.FailureAsync("UserName or password is wrong");
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            if (userRoles.Contains(Roles.Trainee) && userRoles.Count() == 1)
            {
                return await Response.FailureAsync("Account role forbidden.", HttpStatusCode.Forbidden);
            }

            var signInResult = await _signInManager.PasswordSignInAsync(user, query.Password, false, true);

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

            var response = user.Adapt<MobileLoginQueryDto>();

            response.Token = _authServices.GenerateToken(user, roles, false);
            response.Roles = roles;

            return await Response.SuccessAsync(response, "Success");
        }
    }
}
