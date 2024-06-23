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
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ISc.Application.Features.Leader.Camps.Commands.Update
{
    public record UpdateCampCommand : IRequest<Response>
    {
        public int id { get; set; }
        public string Name { get; set; }
        public DateOnly startDate { get; set; }
        public DateOnly EndDate { get; set; }
        public Term? Term { get; set; }
        public int DurationInWeeks { get; set; }
        public bool OpenForRegister { get; set; }
        public ICollection<string>? MentorsIds { get; set; }
        public ICollection<string>? HeadsIds { get; set; }
    }

    internal class UpdateCampCommandHandler : IRequestHandler<UpdateCampCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<Account> _userManager;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateCampCommand> _validator;

        public UpdateCampCommandHandler(
            IUnitOfWork unitOfWork,
            IValidator<UpdateCampCommand> validator,
            UserManager<Account> userManager,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<Response> Handle(UpdateCampCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                return await Response.ValidationFailureAsync(validationResult.Errors.ToList(), HttpStatusCode.UnprocessableEntity);
            }

            var camp = await _unitOfWork.Repository<Camp>().GetByIdAsync(command.id);

            if (camp is null)
            {
                return await Response.FailureAsync("Camp not found.");
            }

            _mapper.Map(command, camp);

            await UpdateMentors(command, cancellationToken);
            await UpdateHeads(command, cancellationToken);

            await _unitOfWork.Repository<Camp>().UpdateAsync(camp);
            await _unitOfWork.SaveAsync();


            return await Response.SuccessAsync(camp.Id, "Camp edit successfully.");
        }

        private async Task UpdateHeads(UpdateCampCommand command, CancellationToken cancellationToken)
        {

            var heads = await _unitOfWork.Heads.Entities
                                .Where(x => x.CampId == command.id)
                                .ToListAsync(cancellationToken);

            if (command.HeadsIds == null || command.HeadsIds.Count == 0)
            {
                foreach (var head in heads)
                {
                    await _unitOfWork.Heads.Delete(head.Account, head);
                }

                return;
            }

            var deletedHeads = heads.Where(x => !command.HeadsIds.Contains(x.Id)).ToList();
            var newHeadsIds = command.HeadsIds.Where(x => !heads.Select(h => h.Id).Contains(x)).ToList();

            foreach (var headId in newHeadsIds)
            {
                var head = await _unitOfWork.Heads.Entities.FirstOrDefaultAsync(x => x.Id == headId && x.CampId != command.id, cancellationToken);

                if (head != null)
                {
                    head.CampId = command.id;
                    await _unitOfWork.Heads.UpdateAsync(new() { Member = head });
                }
                else
                {
                    var newHead = new HeadOfCamp() { Id = headId, CampId = command.id };
                    await _unitOfWork.Heads.AddAsync(new() { Member = newHead });

                    var user = await _userManager.FindByIdAsync(newHead.Id);
                    await _userManager.AddToRoleAsync(user!, Roles.Head_Of_Camp);
                }
            }

            foreach (var head in deletedHeads)
            {
                await _unitOfWork.Heads.Delete(head.Account, head);
            }
        }

        private async Task UpdateMentors(UpdateCampCommand command, CancellationToken cancellationToken)
        {

            var mentors = await _unitOfWork.Repository<MentorsOfCamp>().Entities
                                          .Where(x => x.CampId == command.id)
                                          .ToListAsync(cancellationToken);

            if (command.MentorsIds is null || command.MentorsIds.Count == 0)
            {
                await _unitOfWork.Trainees.Entities
                .Where(x => mentors.Select(x => x.MentorId).Contains(x.MentorId))
                .ForEachAsync(x => x.MentorId = null, cancellationToken);

                _unitOfWork.Repository<MentorsOfCamp>().DeleteRange(mentors);
                return;
            }

            var deletedMentors = mentors.Where(x => !command.MentorsIds.Contains(x.MentorId)).ToList();
            var newMentors = command.MentorsIds.Where(x => !mentors.Select(x => x.MentorId).Contains(x))
                            .Select(x => new MentorsOfCamp()
                            {
                                CampId = command.id,
                                MentorId = x
                            }).ToList();

            await _unitOfWork.Repository<MentorsOfCamp>().AddRangeAsync(newMentors);
            _unitOfWork.Repository<MentorsOfCamp>().DeleteRange(deletedMentors);
        }
    }
}
