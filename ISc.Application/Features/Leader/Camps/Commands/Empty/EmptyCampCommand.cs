using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Shared;
using MediatR;

namespace ISc.Application.Features.Leader.Camps.Commands.Empty
{
    public record EmptyCampCommand : IRequest<Response>
    {
        public int Id { get; set; }

        public EmptyCampCommand(int id)
        {
            Id = id;
        }
    }

    internal class EmptyCampCommandHandler : IRequestHandler<EmptyCampCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmptyCampCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(EmptyCampCommand command, CancellationToken cancellationToken)
        {
            var camp = await _unitOfWork.Repository<Camp>().GetByIdAsync(command.Id);

            if (camp is null)
            {
                return await Response.FailureAsync("Camp not found.");
            }

            var trainees = camp.Trainees.ToList();
            var mentors = camp.Mentors.ToList();
            var heads = camp.Heads.ToList();
            var isCompleted = DateOnly.FromDateTime(DateTime.Now) >= camp.EndDate;

            foreach (var trainee in trainees)
            {
                await _unitOfWork.Trainees.DeleteAsync(trainee.Account, trainee, isCompleted);
            }

            foreach (var mentor in mentors)
            {
                var entity = mentor.Mentor;
                await _unitOfWork.Mentors.Delete(entity.Account, entity, camp.Id);
            }

            foreach (var head in heads)
            {
                await _unitOfWork.Heads.Delete(head.Account, head);
            }

            camp.OpenForRegister = false;

            await _unitOfWork.Repository<Camp>().UpdateAsync(camp);
            await _unitOfWork.SaveAsync();

            return await Response.SuccessAsync("Camp became empty.");
        }
    }
}
