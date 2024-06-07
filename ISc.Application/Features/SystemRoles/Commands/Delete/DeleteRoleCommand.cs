using FluentValidation;
using ISc.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ISc.Application.Features.SystemRoles.Commands.Delete
{
    public record DeleteRoleCommand : IRequest<Response>
    {
        public string Name { get; set; }

        public DeleteRoleCommand(string name)
        {
            Name = name;
        }
    }

    internal class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, Response>
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IValidator<DeleteRoleCommand> _validator;

        public DeleteRoleCommandHandler(RoleManager<IdentityRole> roleManager, IValidator<DeleteRoleCommand> validator)
        {
            _roleManager = roleManager;
            _validator = validator;
        }

        public async Task<Response> Handle(DeleteRoleCommand command, CancellationToken cancellationToken)
        {
            var validationResult=await _validator.ValidateAsync(command);

            if(!validationResult.IsValid)
            {
                return await Response.ValidationFailureAsync(validationResult.Errors.ToList(),System.Net.HttpStatusCode.Forbidden);
            }

            var role = await _roleManager.FindByNameAsync(command.Name);

            if (role == null)
            {
                return await Response.FailureAsync("role not found", System.Net.HttpStatusCode.NotFound);
            }
            var result = await _roleManager.DeleteAsync(role);

            if (!result.Succeeded)
            {
                return await Response.ValidationFailureAsync(result.Errors.ToList(), System.Net.HttpStatusCode.InternalServerError);
            }

            return await Response.SuccessAsync("Role deleted.");
        }
    }
}
