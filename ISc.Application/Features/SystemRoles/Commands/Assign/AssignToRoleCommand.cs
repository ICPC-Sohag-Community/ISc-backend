using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Comman.Constant;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ISc.Application.Features.SystemRoles.Commands.Assign
{
    public record AssignToRoleCommand:IRequest<Response>
    {
        public string userId { get; set; }
        public string Role { get; set; }
        public int? CampId { get; set; }
    }

    internal class AssignToRoleCommandHandler : IRequestHandler<AssignToRoleCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<Account> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AssignToRoleCommandHandler(
            IUnitOfWork unitOfWork,
            UserManager<Account> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<Response> Handle(AssignToRoleCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(command.userId);

            if (user == null)
            {
                return await Response.FailureAsync("User not found.");
            }

            var roleFound = await _roleManager.FindByNameAsync(command.Role);

            if (roleFound is null)
            {
                return await Response.FailureAsync("Role is not correct.");
            }

            var isInRole = await _userManager.IsInRoleAsync(user, command.Role);

            if (isInRole)
            {
                return await Response.FailureAsync($"User already assigned to role {command.Role}");
            }

            if ((command.Role == Roles.Mentor || command.Role == Roles.Trainee || command.Role == Roles.Head_Of_Camp) && command.CampId is null)
            {
                return await Response.FailureAsync("Invalid reqeust");
            }

            if(command.Role==Roles.Mentor)
            {
                await addToMentor(user, (int)command.CampId!);
            }
            else if(command.Role == Roles.Trainee)
            {
                await addToTrainee(user, (int)command.CampId!);
            }
            else if(command.Role==Roles.Head_Of_Camp)
            {
                await addToHead(user, (int)command.CampId!);
            }
            else
            {
                await _userManager.AddToRoleAsync(user, command.Role);
            }
            await _unitOfWork.SaveAsync();

            return await Response.SuccessAsync("User added to role.");
        }

        private async Task addToMentor(Account user, int campId)
        {
            var mentor = await _unitOfWork.Mentors.GetByIdAsync(user.Id);
            

            if (await _unitOfWork.Repository<MentorsOfCamp>().Entities.AnyAsync(x => x.CampId == campId && x.MentorId == user.Id))
            {
                return;
            }

            if (mentor == null)
            {
                mentor = new()
                {
                    Id = user.Id
                };

                await _unitOfWork.Mentors.AddAsync(new() { Account = user, Member = mentor });
                await _unitOfWork.SaveAsync();
            }


            await _unitOfWork.Repository<MentorsOfCamp>().AddAsync(new()
            {
                CampId = campId,
                MentorId = user.Id
            });
        }

        private async Task addToTrainee(Account user, int campId)
        {
            var trainee = await _unitOfWork.Trainees.GetByIdAsync(user.Id);

            if(trainee is null)
            {
                await _unitOfWork.Trainees.AddAsync(new()
                {
                    Account = user,
                    Member = new Trainee()
                    {
                        Id = user.Id,
                        CampId = campId,
                    },
                });
            }
            else
            {
                trainee.CampId = campId;
                await _unitOfWork.Trainees.UpdateAsync(new()
                {
                    Account = user,
                    Member = trainee
                });

                await _unitOfWork.Trainees.UpdateAsync(new()
                {
                    Account = user,
                    Member = trainee
                });
            }
        }

        private async Task addToHead(Account user, int campId)
        {
            var head = await _unitOfWork.Heads.GetByIdAsync(user.Id);

            if(head is null)
            {
                await _unitOfWork.Heads.AddAsync(new()
                {
                    Account = user,
                    Member = new()
                    {
                        Id = user.Id,
                        CampId = campId
                    }
                });
            }
            else
            {
                head.CampId = campId;

                await _unitOfWork.Heads.UpdateAsync(new()
                {
                    Account = user,
                    Member = head
                });
            }
        }
    }
}
