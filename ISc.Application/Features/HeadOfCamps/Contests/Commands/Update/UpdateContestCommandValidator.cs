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
            
        }
    }
}
