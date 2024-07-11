using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.Authentication.CheckResetOtp
{
    public class CheckResetOtpQueryValidator:AbstractValidator<CheckResetOtpQuery>
    {
        public CheckResetOtpQueryValidator()
        {
            RuleFor(x=>x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Otp).NotEmpty();
        }
    }
}
