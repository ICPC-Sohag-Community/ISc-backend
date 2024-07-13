using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.HeadOfCamps.Materials.Commands.Update
{
	public class UpdateMateriaCommandValidator:AbstractValidator<UpdateMateriaCommand>
	{
        public UpdateMateriaCommandValidator()
        {
			RuleFor(x=>x.Title).NotEmpty().MaximumLength(100);
		}
	}
}
