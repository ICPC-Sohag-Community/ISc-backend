using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ISc.Application.Features.CampsModels.Commands.Create
{
    public class CreateCampModelCommandValidator:AbstractValidator<CreateCampModelCommand>
    {
        public CreateCampModelCommandValidator()
        {
            RuleFor(x => x.Name).MaximumLength(40);
        }
    }
}
