using FluentValidation;
using ISc.Application.Interfaces;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Comman.Constant;
using ISc.Domain.Comman.Enums;
using ISc.Domain.Models;
using ISc.Domain.Models.CommunityStaff;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Specialized;

namespace ISc.Application.Features.Leader.Accounts.Commands.Create
{
    public record CreateAccountCommand : IRequest<Response>
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string NationalId { get; set; }
        public DateOnly BirthDate { get; set; }
        public string? PhoneNumber { get; set; }
        public College College { get; set; }
        public string CodeForceHandle { get; set; }
        public string? FacebookLink { get; set; }
        public int Grade { get; set; }
        public Gender Gender { get; set; }
        public IFormFile? ProfileImage { get; set; }
        public string? VjudgeHandle { get; set; }
        public int? CampId { get; set; }
        public string Role { get; set; }
    }
    internal class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, Response>
    {
        private readonly UserManager<Account> _userManager;
        private readonly IValidator<CreateAccountCommand> _validator;
        private readonly IMediaServices _mediaServices;
        private readonly IEmailSender _emailSender;
        private readonly IOnlineJudgeServices _onlineJudgeServices;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHelperService _helperService;
        public CreateAccountCommandHandler(
            UserManager<Account> userManager,
            IValidator<CreateAccountCommand> validator,
            IMediaServices mediaServices,
            IEmailSender emailSender,
            IOnlineJudgeServices onlineJudgeServices,
            RoleManager<IdentityRole> roleManager,
            IUnitOfWork unitOfWork,
            IHelperService helperService)
        {
            _userManager = userManager;
            _validator = validator;
            _mediaServices = mediaServices;
            _emailSender = emailSender;
            _onlineJudgeServices = onlineJudgeServices;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _helperService = helperService;
        }

        public async Task<Response> Handle(CreateAccountCommand command, CancellationToken cancellationToken)
        {
            var validation = await _validator.ValidateAsync(command, cancellationToken);

            if (!validation.IsValid)
            {
                return await Response.ValidationFailureAsync(validation.Errors, System.Net.HttpStatusCode.UnprocessableEntity);
            }
            else if ((command.Role == Roles.Trainee && command.CampId is null) || (command.Role == Roles.Head_Of_Camp && command.CampId is null))
            {
                return await Response.FailureAsync("Role must follow with camp");
            }
            else if (!await _roleManager.RoleExistsAsync(command.Role))
            {
                return await Response.FailureAsync("Role not found...please stop hacking the website");
            }
            else if (command.CampId != null && await _unitOfWork.Repository<Camp>().GetByIdAsync((int)command.CampId) == null)
            {
                return await Response.FailureAsync("Camp not found.");
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.NationalId == command.NationalId || x.Email == command.Email
                        || x.CodeForceHandle == command.CodeForceHandle
                        || x.VjudgeHandle != null && command.VjudgeHandle != null && x.VjudgeHandle == command.VjudgeHandle
                        || command.PhoneNumber != null && x.PhoneNumber == command.PhoneNumber);


            if (user is null)
            {
                if (!await _onlineJudgeServices.ValidateHandleAsync(command.CodeForceHandle))
                {
                    return await Response.FailureAsync("Invalid codeforce handle.");
                }

                await AddNewUser(command);
            }
            else
            {
                return await Response.FailureAsync("User already exist");
			}
            
            return await Response.SuccessAsync("User added successfully.");
        }

        private async Task AddNewUser(CreateAccountCommand command)
        {
            var account = command.Adapt<Account>();

            account.UserName = _helperService.GetRandomUserNameString(command.FirstName, command.NationalId);
            var password = _helperService.GetRandomPasswordString(command.FirstName, command.NationalId);

            if (command.Role == Roles.Trainee)
            {
                Trainee trainee = new Trainee
                {
                    Id = account.Id,
                    CampId = (int)command.CampId!
                };

                await _unitOfWork.Trainees.AddAsync(new() { Account = account, Member = trainee, Password = password });
            }
            else if (command.Role == Roles.Mentor)
            {
                Mentor mentor = new() { Id = account.Id };

                await _unitOfWork.Mentors.AddAsync(new() { Account = account, Member = mentor, Password = password });

            }
            else if (command.Role == Roles.Head_Of_Camp)
            {
                HeadOfCamp head = new HeadOfCamp
                {
                    Id = account.Id,
                    CampId = (int)command.CampId!
                };

                await _unitOfWork.Heads.AddAsync(new() { Account = account, Member = head, Password = password });
            }
            else
            {
                var result = await _userManager.CreateAsync(account, password);

                if (!result.Succeeded) {
                    Console.WriteLine(result.Errors.First().ToString);
                }
                await _userManager.AddToRoleAsync(account, command.Role);
            }

            await _unitOfWork.SaveAsync();

            await _emailSender.SendAccountInfoEmailAsync(account, password, command.Role);

            if (command.ProfileImage != null)
            {
                account.PhotoUrl = await _mediaServices.SaveAsync(command.ProfileImage);
            }
        }
    }
}
