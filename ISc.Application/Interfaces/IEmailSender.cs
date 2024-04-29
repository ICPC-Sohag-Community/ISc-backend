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
        Task<bool> SendRejectionEmailAsync(string email, string ApplicantName);
        Task<bool> SendAcceptTraineeEmailAsync(Trainee trainee,string campName,DateOnly StartDate);
        Task<bool> SendKickedoutEmailAsync(string email, string CampName, string TraineeName);
        Task<bool> SendAccountInfoAsync(string userName, string Password, Trainee trainee);
        Task<bool> SendForgetPassword(string email, string Name, int Otp);
    }
}
