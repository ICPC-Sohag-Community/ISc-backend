using ISc.Application.Interfaces;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;

namespace ISc.Application.Features.Authentication.SendConfirmEmailOtp
{
    public class SendConfirmEmailOtpCommand:IRequest<Response>
    {
        public string Email { get; set; }

        public SendConfirmEmailOtpCommand(string email)
        {
            Email = email;
        }
    }
    public class SendConfirmEmailOtpCommandHandler : IRequestHandler<SendConfirmEmailOtpCommand, Response>
    {
        private readonly IEmailSender _emailSender;  
        private readonly IDistributedCache _cache;
        private readonly UserManager<Account> _userManager;

        public SendConfirmEmailOtpCommandHandler(IDistributedCache cache, IEmailSender emailSender, UserManager<Account> userManager)
        {
            _cache = cache;
            _emailSender = emailSender;
            _userManager = userManager;
        }

        public async Task<Response> Handle(SendConfirmEmailOtpCommand command, CancellationToken cancellationToken)
        {
            var user=await _userManager.FindByEmailAsync(command.Email);  

            if (user == null)
            {
                return await Response.FailureAsync("Invalid Email");  

            }

            var rand=new Random();
            var otp = rand.Next(99999, 1000000);


            if(!await _emailSender.SendEmailConfirmationAsync(command.Email, otp))
            {
                return await Response.FailureAsync("Couldn't send email confirmation otp");
            }

            _cache.SetString(command.Email, otp.ToString());

            return await Response.SuccessAsync("Success");    
        }
    }
}
