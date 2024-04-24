using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISc.Application.Dtos.Email;

namespace ISc.Application.Interfaces
{
    public interface IEmailSender
    {
        Task<bool> SendRejectionEmailAsync();
        Task<bool> SendAcceptTraineeEmailAsync();
        Task<bool> SendKickedoutEmailAsync();
        Task<bool> SendAccountInfoAsync();
        Task<bool> SendForgetPassword(int otp);
    }
}
