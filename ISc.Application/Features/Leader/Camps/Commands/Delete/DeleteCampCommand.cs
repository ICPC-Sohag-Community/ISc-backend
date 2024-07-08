using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Shared;
using MediatR;

namespace ISc.Application.Features.Leader.Camps.Commands.Delete
{
    public record DeleteCampCommand:IRequest<Response>
    {
        public int Id { get; set; }

        public DeleteCampCommand(int id)
        {
            Id = id;
        }
    }

    internal class DeleteCampCommandHandler : IRequestHandler<DeleteCampCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCampCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(DeleteCampCommand command, CancellationToken cancellationToken)
        {
            var camp = await _unitOfWork.Repository<Camp>().GetByIdAsync(command.Id);

            if(camp is null)
            {
                return await Response.FailureAsync("Camp not found.");
            }

            var trainees = camp.Trainees.ToList();
            var isCompleted = DateOnly.FromDateTime(DateTime.Now) >= camp.EndDate;

            foreach(var trainee in trainees)
            {
                await _unitOfWork.Trainees.DeleteAsync(trainee.Account, trainee, isCompleted);
            }

            _unitOfWork.Repository<Camp>().Delete(camp);
            await _unitOfWork.SaveAsync();

            return await Response.SuccessAsync("Camp deleted.");
        }
    }
}
