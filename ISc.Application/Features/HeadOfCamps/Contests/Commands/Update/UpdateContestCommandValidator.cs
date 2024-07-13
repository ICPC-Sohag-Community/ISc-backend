using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.HeadOfCamps.Contests.Commands.Update
{
    public class UpdateContestCommandValidator:AbstractValidator<UpdateContestCommand>
    {
        public UpdateContestCommandValidator()
        {
            RuleFor(x => x.CodeForceId).NotEmpty();
            RuleFor(x=>x.Name).NotEmpty();
            RuleFor(x=>x.Link).NotEmpty();
            RuleFor(x=>x.StartTime).NotEmpty();
            RuleFor(x => x).Must(x => x.StartTime <= x.EndTime).WithMessage("Start time must be less than end time.");
        }
    }
}
