using System.Net;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace ISc.Application.Features.Authentication.ResetPassword
{
    public record ResetPasswordCommand : IRequest<Response>
    {
        public string Password { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
    }

    internal class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Response>
    {
        private readonly UserManager<Account> _userManager;
        private readonly IMemoryCache _memoryCache;

        public ResetPasswordCommandHandler(
            UserManager<Account> userManager,
            IMemoryCache memoryCache)
        {
            _userManager = userManager;
            _memoryCache = memoryCache;
        }

        public async Task<Response> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(command.Email);

            if (user == null)
            {
                return await Response.FailureAsync("No account with this email.");
            }

            var resetPasswordResult = await _userManager.ResetPasswordAsync(user, command.Token, command.Password);


            if (!resetPasswordResult.Succeeded)
            {
                return await Response.ValidationFailureAsync(resetPasswordResult.Errors.ToList(), HttpStatusCode.Unauthorized);
            }

            return await Response.SuccessAsync(user.Id, "Password changed.");
        }
    }
}
