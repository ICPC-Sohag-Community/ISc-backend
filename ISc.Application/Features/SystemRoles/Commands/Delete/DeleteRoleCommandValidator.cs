using FluentValidation;
using ISc.Domain.Comman.Constant;

namespace ISc.Application.Features.SystemRoles.Commands.Delete
{
    public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
    {
        public DeleteRoleCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Must(x => x != Roles.Trainee
                                               && x != Roles.Mentor
                                               && x != Roles.Head_Of_Camp
                                               && x != Roles.Leader)
                                         .WithMessage("Not allowed to delete this role.");
        }
    }
}
