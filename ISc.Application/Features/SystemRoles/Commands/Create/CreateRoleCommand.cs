using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using ISc.Application.Interfaces.Repos;
using ISc.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ISc.Application.Features.SystemRoles.Commands.Create
{
    public record CreateRoleCommand:IRequest<Response>
    {
        public string Name { get; set; }

        public CreateRoleCommand(string name)
        {
            Name = name;
        }
    }

    internal class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Response>
    {
        private readonly IValidator<CreateRoleCommand> _validator;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CreateRoleCommandHandler(
            IValidator<CreateRoleCommand> validator,
            RoleManager<IdentityRole> roleManager)
        {
            _validator = validator;
            _roleManager = roleManager;
        }

        public async Task<Response> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
            {
                return await Response.ValidationFailureAsync(validationResult.Errors.ToList(), System.Net.HttpStatusCode.UnprocessableEntity);
            }

            var entity = await _roleManager.FindByNameAsync(command.Name);

            if(entity is not null)
            {
                return await Response.FailureAsync("Role already exist.");
            }

            var result= await _roleManager.CreateAsync(new IdentityRole() { Name=command.Name });

            if (!result.Succeeded)
            {
                await Response.ValidationFailureAsync(result.Errors.ToList(),System.Net.HttpStatusCode.InternalServerError);
            }

            return await Response.SuccessAsync("Role add successfully.");
        }
    }
}
