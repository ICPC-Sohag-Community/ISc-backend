using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.HeadOfCamps.Sheets.Commands.Create
{
    public class CreateSheetCommandValidator:AbstractValidator<CreateSheetCommand>
    {
        public CreateSheetCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x=>x.SheetLink).NotEmpty();
            RuleFor(x => x.MinimumPassingPrecent).Must(x => x > 0);
            RuleFor(x => x.SheetCodefroceId).NotEmpty();
            RuleFor(x => x.Community).NotEmpty();
            RuleFor(x=>x.StartDate).NotEmpty();
            RuleFor(x => x).Must(x => x.StartDate <= x.EndDate).When(x => x.EndDate != null);
            RuleFor(x=>x.Status).NotEmpty();
        }
    }
}
