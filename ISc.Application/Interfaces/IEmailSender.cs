using ISc.Domain.Models.IdentityModels;

namespace ISc.Application.Interfaces
{
    public interface IEmailSender
    {
        Task<bool> SendRejectionEmailAsync(string email, string applicantName);
        Task<bool> SendEmailConfirmationAsync(string email, int otp);
        Task<bool> SendAcceptTraineeEmailAsync(Account trainee, string campName, DateOnly startDate);
        Task<bool> SendAcceptToRoleEmailAsync(string email, string name, string role);
        Task<bool> SendKickedoutEmailAsync(string email, string campName, string traineeName);
        Task<bool> SendAccountInfoEmailAsync(Account trainee, string password, string role);
        Task<bool> SendForgetPasswordEmailAsync(string email, string name, int otp);
        Task<bool> SendTraineeRejectionEmailAsync(string email, string fullName);
    }
}
