using ISc.Application.Interfaces;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace ISc.Application.Features.Authentication.Login
{
    public class LoginQuery:IRequest<Response>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class LoginQueryHandler : IRequestHandler<LoginQuery, Response>
    {
        private readonly UserManager<Account> _userManager;
        private readonly IAuthServices _authServices;

        public LoginQueryHandler(UserManager<Account> userManager, IAuthServices authServices)
        {
            _userManager = userManager;
            _authServices = authServices;
        }

        public async Task<Response> Handle(LoginQuery query, CancellationToken cancellationToken)
        {
            var user=await _userManager.FindByNameAsync(query.UserName);

            if (user == null||!await _userManager.CheckPasswordAsync(user,query.Password)) {

                return await Response.FailureAsync("UserName OR Password Is Wrong");
            }
            var roles = await _userManager.GetRolesAsync(user);

            var response = user.Adapt<LoginQueryResponse>();

            response.Token = _authServices.GenerateToken(user, roles);
            response.Roles = roles;
            return await Response.SuccessAsync(response , "Success");
            

        }
    }
}
