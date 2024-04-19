using ISc.Application.Dtos.Email;

namespace ISc.Application.Interfaces
{
    public interface IEmailServices
    {
        Task<bool> SendMailUsingRazorTemplateAsync(EmailRequestDto request);
    }
}
