using FluentValidation;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models.IdentityModels;
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
        private readonly UserManager<Account> _userManager;
        private readonly IStuffArchiveRepo _stuffArchiveRepo;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteRoleCommandHandler(
            RoleManager<IdentityRole> roleManager,
            IValidator<DeleteRoleCommand> validator,
            UserManager<Account> userManager,
            IStuffArchiveRepo stuffArchiveRepo,
            IUnitOfWork unitOfWork)
        {
            _roleManager = roleManager;
            _validator = validator;
            _userManager = userManager;
            _stuffArchiveRepo = stuffArchiveRepo;
            _unitOfWork = unitOfWork;
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

            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);

            foreach (var user in usersInRole)
            {
                await _stuffArchiveRepo.AddToArchiveAsync(user, role.Name!);
                await _userManager.DeleteAsync(user);
            }

            var result = await _roleManager.DeleteAsync(role);

            if (!result.Succeeded)
            {
                return await Response.ValidationFailureAsync(result.Errors.ToList(), System.Net.HttpStatusCode.InternalServerError);
            }

            await _unitOfWork.SaveAsync();

            return await Response.SuccessAsync("Role deleted.");
        }
    }
}
