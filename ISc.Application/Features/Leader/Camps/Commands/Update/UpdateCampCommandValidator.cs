using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ISc.Application.Features.Leader.Camps.Commands.Update
{
    public class UpdateCampCommandValidator:AbstractValidator<UpdateCampCommand>
    {
        public UpdateCampCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(30);
            RuleFor(x => x.OpenForRegister).NotNull();
            RuleFor(x => new { x.startDate, x.EndDate }).Must(x => x.startDate < x.EndDate).WithMessage("In correct Data period");
            RuleFor(x => x.Term).IsInEnum();
        }
    }
}
