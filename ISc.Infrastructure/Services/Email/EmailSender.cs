using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISc.Application.Interfaces;

namespace ISc.Infrastructure.Services.Email
{
    internal class EmailSender : IEmailSender
    {
        public Task<bool> SendAcceptTraineeEmailAsync()
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
