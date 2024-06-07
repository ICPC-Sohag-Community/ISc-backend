using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using ISc.Domain.Comman.Enums;

namespace ISc.Application.Features.Leader.Camps.Commands.Create
{
    public class CreateCampCommandValidator:AbstractValidator<CreateCampCommand>
    {
        public CreateCampCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(30);
            RuleFor(x => x.OpenForRegister).NotEmpty();
            RuleFor(x => new { x.startDate, x.EndDate }).Must(x => x.startDate < x.EndDate).WithMessage("In correct Data period");
            RuleFor(x => x.Term).IsInEnum();
        }
    }
}
