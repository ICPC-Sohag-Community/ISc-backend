using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.Leader.Archives.Commands.DeleteTraineeById
{
	public record DeleteTraineeArchiveByIdCommand:IRequest<Response>
	{
        public int Id { get; set; }

        public DeleteTraineeArchiveByIdCommand(int id)
        {
			Id = id;
        }
    }
	internal class DeleteTraineeByIdCommandHandler : IRequestHandler<DeleteTraineeArchiveByIdCommand, Response>
	{
		private readonly IUnitOfWork _unitOfWork;

		public DeleteTraineeByIdCommandHandler(IUnitOfWork unitofwork)
		{
			_unitOfWork = unitofwork;
		}

		public async Task<Response> Handle(DeleteTraineeArchiveByIdCommand command, CancellationToken cancellationToken)
		{
			var archive = await _unitOfWork.Repository<TraineeArchive>().GetByIdAsync(command.Id);

			if(archive == null)
			{

				return await Response.FailureAsync("Archive not found.", HttpStatusCode.NotFound);
			}

			_unitOfWork.Repository<TraineeArchive>().Delete(archive);
			await _unitOfWork.SaveAsync();

			return await Response.SuccessAsync("archive deleted.");
		}
	}
}
