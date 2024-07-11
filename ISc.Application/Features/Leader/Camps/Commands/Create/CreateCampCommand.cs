using System.Net;
using FluentValidation;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Comman.Constant;
using ISc.Domain.Comman.Enums;
using ISc.Domain.Models;
using ISc.Domain.Models.CommunityStaff;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
        public ICollection<string>? MentorsIds { get; set; }
        public ICollection<string>? HeadsIds { get; set; }
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

            if (await _unitOfWork.Repository<Camp>().Entities.AnyAsync(x => x.Name.ToLower().Trim() == command.Name.ToLower().Trim()))
            {
                return await Response.FailureAsync("Camp already exist.");
            }

            if (!command.MentorsIds.IsNullOrEmpty()
                && command.MentorsIds!.Any(x => !_unitOfWork.Mentors.Entities.Select(m => m.Id).Contains(x)))
            {
                return await Response.FailureAsync("Some mentors not valid.");
            }

            var camp = command.Adapt<Camp>();

            await _unitOfWork.Repository<Camp>().AddAsync(camp);
            await _unitOfWork.SaveAsync();

            if (!command.MentorsIds.IsNullOrEmpty())
            {
                await _unitOfWork.Repository<MentorsOfCamp>().AddRangeAsync(command.MentorsIds!.Select(x => new MentorsOfCamp()
                {
                    CampId = camp.Id,
                    MentorId = x
                }).ToList());
            }

            if (!command.HeadsIds.IsNullOrEmpty())
            {
                var existHeads = await _unitOfWork.Heads.Entities
                       .Where(x => command.HeadsIds!.Contains(x.Id))
                       .ToListAsync();

                foreach (var head in existHeads)
                {
                    head.CampId = camp.Id;
                    await _unitOfWork.Heads.UpdateAsync(new() { Member = head });
                }

                var newHeadsIds = command.HeadsIds!.Except(existHeads.Select(x => x.Id)).ToList();

                foreach (var headId in newHeadsIds)
                {
                    var newHead = new HeadOfCamp() { Id = headId, CampId = camp.Id };
                    var account = await _userManager.FindByIdAsync(headId);

                    await _unitOfWork.Heads.AddAsync(new() { Member = newHead, Account = account });
                }
            }

            await _unitOfWork.SaveAsync();

            return await Response.SuccessAsync(camp.Id,"Camp added successfully.");
        }
    }
}
