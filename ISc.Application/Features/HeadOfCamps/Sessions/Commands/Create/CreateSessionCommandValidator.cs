using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.HeadOfCamps.Sessions.Commands.Create
{
	public class CreateSessionCommandValidator:AbstractValidator<CreateSessionCommand>
	{
        public CreateSessionCommandValidator()
        {
            RuleFor(x => x.Topic).NotEmpty().MaximumLength(50);
            RuleFor(x => x.InstructorName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.LocationLink).NotEmpty();
            RuleFor(x => x.LocationName).NotEmpty();
			RuleFor(x => x.StartDate).NotEmpty();
            RuleFor(x => x).Must(x => x.StartDate <= x.EndDate).When(x => x.EndDate != null);

		}
    }
}
