using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ISc.Application.Features.CampsModels.Commands.Delete
{
    public record DeleteCampModelCommand:IRequest<Response>
    {
        public string Name { get;set; }

        public DeleteCampModelCommand(string name)
        {
            Name = name;
        }
    }

    internal class DeleteCampModelCommandHandler : IRequestHandler<DeleteCampModelCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCampModelCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(DeleteCampModelCommand query, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repository<CampModel>().Entities.FirstOrDefaultAsync(x => x.Name == query.Name);

            if(entity is null)
            {
                return await Response.FailureAsync("No item found.");
            }

            _unitOfWork.Repository<CampModel>().Delete(entity);
            await _unitOfWork.SaveAsync();

            return await Response.SuccessAsync("Model deleted.");
        }
    }
}
