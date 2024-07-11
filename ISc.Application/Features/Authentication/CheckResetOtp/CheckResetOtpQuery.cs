using FluentValidation;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using System.Net;

namespace ISc.Application.Features.Authentication.CheckResetOtp
{
    public record CheckResetOtpQuery : IRequest<Response>
    {
        public string Email { get; set; }
        public int Otp { get; set; }
    }

    internal class CheckResetOtpQueryHandler : IRequestHandler<CheckResetOtpQuery, Response>
    {
        private readonly UserManager<Account> _userManager;
        private readonly IMemoryCache _memoryCache;
        private readonly IValidator<CheckResetOtpQuery> _validator;

        public CheckResetOtpQueryHandler(
            UserManager<Account> userManager,
            IMemoryCache memoryCache,
            IValidator<CheckResetOtpQuery> validator)
        {
            _userManager = userManager;
            _memoryCache = memoryCache;
            _validator = validator;
        }

        public async Task<Response> Handle(CheckResetOtpQuery query, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(query);

            if (!validationResult.IsValid)
            {
                return await Response.ValidationFailureAsync(validationResult.Errors.ToList(), HttpStatusCode.UnprocessableEntity);
            }

            var user = await _userManager.FindByEmailAsync(query.Email);

            if (user == null)
            {
                return await Response.FailureAsync("No account with this email.");
            }

            var otp = _memoryCache.Get<int>("Reset" + user.Id);


            if (otp != query.Otp)
            {
                return await Response.FailureAsync("Otp is incorrect.");
            }

            _memoryCache.Remove("Reset" + user.Id);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            return await Response.SuccessAsync(token, "Otp is correct.");
        }
    }
}
