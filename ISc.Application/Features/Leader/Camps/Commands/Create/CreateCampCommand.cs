using System.Net;
using FluentValidation;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Comman.Constant;
using ISc.Domain.Comman.Enums;
using ISc.Domain.Models;
using ISc.Domain.Models.CommunityStuff;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ISc.Application.Features.Leader.Camps.Commands.Create
{
    public record CreateCampCommand : IRequest<Response>
    {
        public string Name { get; set; }
        public DateOnly startDate { get; set; }
        public DateOnly EndDate { get; set; }
        public Term? Term { get; set; }
        public int DurationInWeeks { get; set; }
        public bool OpenForRegister { get; set; }
        public ICollection<string> MentorsIds { get; set; }
        public ICollection<string> HeadsIds { get; set; }
    }

    internal class CreateCampCommandHandler : IRequestHandler<CreateCampCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateCampCommand> _validator;
        private readonly UserManager<Account> _userManager;

        public CreateCampCommandHandler(
            IUnitOfWork unitOfWork,
            IValidator<CreateCampCommand> validator,
            UserManager<Account> userManager)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _userManager = userManager;
        }

        public async Task<Response> Handle(CreateCampCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                return await Response.ValidationFailureAsync(validationResult.Errors.ToList(), HttpStatusCode.UnprocessableEntity);
            }

            if (!await _unitOfWork.Repository<Camp>().Entities.AnyAsync(x => x.Name == command.Name))
            {
                return await Response.FailureAsync("Camp already exist.");
            }

            if (!await _unitOfWork.Mentors.Entities.AnyAsync(x => !command.MentorsIds.Contains(x.Id)))
            {
                return await Response.FailureAsync("Some mentors not valid.");
            }

            if (!await _unitOfWork.Heads.Entities.AnyAsync(x => !command.HeadsIds.Contains(x.Id)))
            {
                return await Response.FailureAsync("Some heads not valid.");
            }

            var camp = command.Adapt<Camp>();

            await _unitOfWork.Repository<Camp>().AddAsync(camp);
            await _unitOfWork.SaveAsync();

            await _unitOfWork.Repository<MentorsOfCamp>().AddRangeAsync(command.MentorsIds.Select(x => new MentorsOfCamp()
            {
                CampId = camp.Id,
                MentorId = x
            }).ToList());

            var existHeads = await _unitOfWork.Heads.Entities
                       .Where(x => command.HeadsIds.Contains(x.Id))
                       .ToListAsync();

            foreach (var head in existHeads)
            {
                head.CampId = camp.Id;
                await _unitOfWork.Heads.UpdateAsync(new() { Member = head });
            }

            var newHeadsIds = command.HeadsIds.Except(existHeads.Select(x => x.Id)).ToList();

            foreach(var headId in newHeadsIds)
            {
                var newHead = new HeadOfCamp() { Id = headId, CampId = camp.Id };
                var account = await _userManager.FindByIdAsync(headId);

                await _unitOfWork.Heads.AddAsync(new() { Member = newHead });

                await _userManager.AddToRoleAsync(account!, Roles.Head_Of_Camp);
            }

            await _unitOfWork.SaveAsync();

            return await Response.SuccessAsync("Camp added successfully.");
        }
    }
}
