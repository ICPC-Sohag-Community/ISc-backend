using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ISc.Application.Features.Authentication.SendConfirmEmailOtp
{
    public class SendConfirmEmailOtpCommandValidator:AbstractValidator<SendConfirmEmailOtpCommand>
    {
        public SendConfirmEmailOtpCommandValidator()
        {
            RuleFor(x => x.Email).EmailAddress();
        }
    }
}
