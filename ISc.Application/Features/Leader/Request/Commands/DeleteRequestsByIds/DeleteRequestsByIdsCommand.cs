using ISc.Application.Interfaces;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.Leader.Request.Commands.DeleteRequestsByIds
{
    public record DeleteRequestsByIdsCommand:IRequest<Response>
    {
        public List<int> RequestsIds { get; set; }

        public DeleteRequestsByIdsCommand(List<int> requestsIds)
        {
            RequestsIds = requestsIds;
        }
    }

    internal class DeleteRequestsByIdsCommandHandler : IRequestHandler<DeleteRequestsByIdsCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailService;

        public DeleteRequestsByIdsCommandHandler(
            IUnitOfWork unitOfWork,
            IEmailSender emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public async Task<Response> Handle(DeleteRequestsByIdsCommand command, CancellationToken cancellationToken)
        {
            var requests = await _unitOfWork.Repository<NewRegisteration>().Entities
                          .Where(x => command.RequestsIds.Contains(x.Id))
                          .ToListAsync(cancellationToken);

            if(requests.IsNullOrEmpty())
            {
                return await Response.FailureAsync("No requests found.");
            }

            requests.ForEach(x => _emailService.SendTraineeRejectionEmailAsync(x.Email, x.FirstName + ' ' + x.MiddleName)); 

            _unitOfWork.Repository<NewRegisteration>().DeleteRange(requests);
            await _unitOfWork.SaveAsync();

            return await Response.SuccessAsync("Requests deleted.");
        }
    }
}
