using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ISc.Application.Features.SystemRoles.Commands.Create
{
    public class CreateRoleCommandValidator:AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(20);
        }
    }
}
