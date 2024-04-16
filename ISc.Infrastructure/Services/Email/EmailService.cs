using FluentEmail.Core;
using ISc.Application.Dtos.Email;
using ISc.Application.Interfaces;

namespace ISc.Infrastructure.Services.Email
{
    internal class EmailService : IEmailServices
    {
        private readonly IFluentEmail _fluentEmail;

        public EmailService(IFluentEmail fluentEmail)
        {
            _fluentEmail = fluentEmail;
        }

        public async Task<bool> SendMailUsingRazorTemplateAsync(EmailRequestDto request)
        {
            var sendResponse = await _fluentEmail
                                        .To(request.To)
                                        .Subject(request.Subject)
                                        .UsingTemplate(request.Body, request.BodyData)
                                        .SendAsync();

            return sendResponse.Successful;
        }
    }
}
