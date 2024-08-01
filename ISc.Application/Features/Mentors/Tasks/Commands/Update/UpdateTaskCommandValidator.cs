﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.Mentors.Tasks.Commands.Update
{
    public class UpdateTaskCommandValidator:AbstractValidator<UpdateTaskCommand>
    {
        public UpdateTaskCommandValidator()
        {
            RuleFor(x=>x.DeadLine).NotEmpty().Must(x=>x>DateTime.Now);
            RuleFor(x => x.Task).NotEmpty();
            RuleFor(x=>x.TraineeId).NotEmpty();
        }
    }
}
