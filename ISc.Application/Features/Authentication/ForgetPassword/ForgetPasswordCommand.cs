using System.Net;
using FluentValidation;
using ISc.Application.Interfaces;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace ISc.Application.Features.Authentication.ForgetPassword
{
    public record ForgetPasswordCommand : IRequest<Response>
    {
        public string Email { get; set; }

        public ForgetPasswordCommand(string email)
        {
            Email = email;
        }
    }

    internal class ForgetPasswordCommandHandler : IRequestHandler<ForgetPasswordCommand, Response>
    {
        private readonly UserManager<Account> _userManager;
        private readonly IValidator<ForgetPasswordCommand> _validator;
        private readonly IEmailSender _emailSender;
        private readonly IMemoryCache _memoryCache;

        public ForgetPasswordCommandHandler(
            UserManager<Account> userManager,
            IValidator<ForgetPasswordCommand> validator,
            IEmailSender emailSender,
            IMemoryCache memoryCache
            )
        {
            _userManager = userManager;
            _validator = validator;
            _emailSender = emailSender;
            _memoryCache = memoryCache;
        }

        public async Task<Response> Handle(ForgetPasswordCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
            {
                return await Response.ValidationFailureAsync(validationResult.Errors, HttpStatusCode.BadRequest);
            }

            var user = await _userManager.FindByEmailAsync(command.Email);

            if (user is null)
            {
                return await Response.FailureAsync("Email not found");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            _memoryCache.Remove(user.Id);

            int otp = await _memoryCache.GetOrCreateAsync(
                    key: user.Id,
                    cacheEntry =>
                    {
                        cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(3);
                        return Task.FromResult(new Random().Next(1000));
                    });

            var isEmailSent = await _emailSender.SendForgetPassword(command.Email, user.FirstName + ' ' + user.MiddleName, otp);

            if (!isEmailSent)
            {
                _memoryCache.Remove(user.Id);
                return await Response.FailureAsync($"Fail to send otp to {command.Email}");
            }

            return await Response.SuccessAsync(data: token);
        }
    }
}
