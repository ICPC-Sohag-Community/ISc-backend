using ISc.Application.Interfaces;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Comman.Constant;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Principal;

namespace ISc.Application.Features.SystemRoles.Commands.UnAssign
{
	public record UnAssignRoleCommand : IRequest<Response>
	{
		public string UserId { get; set; }
		public List<RoleInfo> RoleInfos { get; set; }
	}
	public record RoleInfo
	{
		public string Role { get; set; }
		public int? CampId { get; set; }
	}
	internal class UnAssignRoleCommandHandler : IRequestHandler<UnAssignRoleCommand, Response>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly UserManager<Account> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IMediaServices _mediaServices;
		private readonly IStuffArchiveRepo _archiveRepo;

		public UnAssignRoleCommandHandler(
			IUnitOfWork unitOfWork,
			UserManager<Account> userManager,
			RoleManager<IdentityRole> roleManager,
			IStuffArchiveRepo archiveRepo,
			IMediaServices mediaServices)
		{
			_unitOfWork = unitOfWork;
			_userManager = userManager;
			_roleManager = roleManager;
			_archiveRepo = archiveRepo;
			_mediaServices = mediaServices;
		}

		public async Task<Response> Handle(UnAssignRoleCommand command, CancellationToken cancellationToken)
		{

			var user = await _userManager.FindByIdAsync(command.UserId);

			if (user == null)
			{
				return await Response.FailureAsync("User not found.", System.Net.HttpStatusCode.NotFound);
			}

			foreach(var roleInfo in command.RoleInfos)
			{
				
				var role = await _roleManager.FindByNameAsync(roleInfo.Role);

				if (role == null)
				{
					return await Response.FailureAsync("Role not found.", System.Net.HttpStatusCode.NotFound);
				}

				var userInRole = await _userManager.IsInRoleAsync(user, roleInfo.Role);

				if (userInRole == false)
				{
					return await Response.FailureAsync("Invalid request.", System.Net.HttpStatusCode.BadRequest);
				}

				if ((roleInfo.Role == Roles.Mentor || roleInfo.Role == Roles.Trainee || roleInfo.Role == Roles.Head_Of_Camp) 
					&& roleInfo.CampId is null)
				{
					return await Response.FailureAsync("Invalid reqeust");
				}

				if (roleInfo.Role == Roles.Mentor)
				{
					var mentor = await _unitOfWork.Mentors.GetByIdAsync(user.Id);

					await _unitOfWork.Mentors.Delete(user, mentor, roleInfo.CampId);

				}
				else if (roleInfo.Role == Roles.Trainee)
				{
					var trainee = await _unitOfWork.Trainees.GetByIdAsync(user.Id);

					await _unitOfWork.Trainees.DeleteAsync(user, trainee, false);
				}
				else if (roleInfo.Role == Roles.Head_Of_Camp)
				{
					var head = await _unitOfWork.Heads.GetByIdAsync(user.Id);

					await _unitOfWork.Heads.Delete(user, head);
				}
				else
				{
					var rolesCount = _userManager.GetRolesAsync(user).Result.Count;

					await _archiveRepo.AddToArchiveAsync(user, roleInfo.Role);
					 
					if (rolesCount == 1)
					{
						if (user.PhotoUrl is not null)
						{
							await _mediaServices.DeleteAsync(user.PhotoUrl);
						}
						await _userManager.DeleteAsync(user);
					}
					else
					{
						await _userManager.RemoveFromRoleAsync(user, roleInfo.Role);
					}
				}
			}
			await _unitOfWork.SaveAsync();

			return await Response.SuccessAsync("User removed form role.");

		}
	}
}
