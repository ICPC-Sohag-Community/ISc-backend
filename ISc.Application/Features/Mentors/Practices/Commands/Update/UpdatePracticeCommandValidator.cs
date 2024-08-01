using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.Mentors.Practices.Commands.Update
{
    public class UpdatePracticeCommandValidator:AbstractValidator<UpdatePracticeCommand>
    {
        public UpdatePracticeCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.MeetingLink).MaximumLength(100).NotEmpty();
            RuleFor(x => x.Time).NotEmpty();
            RuleFor(x => x.State).NotEmpty();
        }
    }
}
