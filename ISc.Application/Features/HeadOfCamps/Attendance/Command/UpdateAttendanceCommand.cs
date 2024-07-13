using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ISc.Application.Features.HeadOfCamps.Attendance.Command
{
    public record UpdateAttendanceCommand : IRequest<Response>
    {
        public string TraineeId { get; set; }
        public int SessionId { get; set; }
    }

    internal class UpdateAttendanceCommandHandler : IRequestHandler<UpdateAttendanceCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<Account> _userManager;

        public UpdateAttendanceCommandHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor,
            UserManager<Account> userManager)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        public async Task<Response> Handle(UpdateAttendanceCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext!.User);

            if (user is null)
            {
                return await Response.FailureAsync("Unauthorized.", HttpStatusCode.Unauthorized);
            }

            var head = await _unitOfWork.Heads.GetByIdAsync(user.Id);

            if (head is null)
            {
                return await Response.FailureAsync("Unauthorized.", HttpStatusCode.Unauthorized);
            }

            if (!await _unitOfWork.Repository<Session>().Entities.AnyAsync(x => x.Id == command.SessionId)
                || !await _unitOfWork.Trainees.Entities.AnyAsync(x => x.Id == command.TraineeId))
            {
                return await Response.FailureAsync("Failed to find item.", HttpStatusCode.BadRequest);
            }

            var attend = await _unitOfWork.Repository<TraineeAttendence>().Entities
                                        .SingleOrDefaultAsync(x => x.TraineeId == command.TraineeId && x.SessionId == command.SessionId);

            if(attend is null)
            {
                await _unitOfWork.Repository<TraineeAttendence>().AddAsync(new()
                {
                    TraineeId = command.TraineeId,
                    SessionId = command.SessionId,
                });
            }
            else
            {
                _unitOfWork.Repository<TraineeAttendence>().Delete(attend);
            }

            await _unitOfWork.SaveAsync();

            return await Response.SuccessAsync("Attendance updated.");
        }
    }
}
