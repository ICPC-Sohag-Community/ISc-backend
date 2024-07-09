using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.HeadOfCamps.Materials.Commands.UpdateMaterialOrder
{
    public class UpdateMaterialOrderCommandValidator:AbstractValidator<UpdateMaterialOrderCommand>
    {
        public UpdateMaterialOrderCommandValidator()
        {
            RuleFor(x => x).Must(x => !x.Materials.Any(z => z.Order < 1));
        }
    }
}
