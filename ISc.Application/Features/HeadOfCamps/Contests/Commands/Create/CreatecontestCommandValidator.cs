using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.HeadOfCamps.Contests.Commands.Create
{
    public class CreatecontestCommandValidator:AbstractValidator<CreatecontestCommand>
    {
        public CreatecontestCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.StartTime).NotEmpty();
            RuleFor(x => x.EndTime).NotEmpty();
            RuleFor(x=>x.CodeForceId).NotEmpty();
            RuleFor(x=>x.ProblemCount).NotEmpty();
            RuleFor(x => x.Community).NotEmpty();
            RuleFor(x => x).Must(x => x.StartTime <= x.EndTime).WithMessage("Start time must be less than end time.");
        }
    }
}
