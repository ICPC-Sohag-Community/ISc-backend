using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISc.Application.Dtos.Email;
using ISc.Domain.Models;

namespace ISc.Application.Interfaces
{
    public interface IEmailSender
    {
        Task<bool> SendRejectionEmailAsync(string email, string applicantName);
        Task<bool> SendEmailConfirmationAsync(string email,int otp);
        Task<bool> SendAcceptTraineeEmailAsync(Trainee trainee,string campName,DateOnly startDate);
        Task<bool> SendKickedoutEmailAsync(string email, string campName, string traineeName);
        Task<bool> SendAccountInfoAsync(string userName, string password, Trainee trainee);
        Task<bool> SendForgetPassword(string email, string name, int otp);
    }
}
