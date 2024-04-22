using ISc.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace ISc.Infrastructure.Services.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;

        public EmailSender(IConfiguration config)
        {
            _config = config;
        }

        public Task<bool> SendAcceptTraineeEmailAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendAccountInfoAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendKickedoutEmailAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendRejectionEmailAsync()
        {
            throw new NotImplementedException();
        }
    }
}
