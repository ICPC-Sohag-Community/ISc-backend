using ISc.Application.Interfaces;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ISc.Application.Features.Leader.Request.Commands.SubmitRequests
{
    public record SubmitRequestsCommand : IRequest<Response>
    {
        public int CampId { get; set; }
        public List<int> RequestsId { get; set; }
    }

    internal class SubmitRequestsCommandHandler : IRequestHandler<SubmitRequestsCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailServices;
        private readonly IHelperService _helperService;

        public SubmitRequestsCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IEmailSender emailServices,
            IHelperService helperService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailServices = emailServices;
            _helperService = helperService;
        }

        public async Task<Response> Handle(SubmitRequestsCommand command, CancellationToken cancellationToken)
        {
            var camp = await _unitOfWork.Repository<Camp>().GetByIdAsync(command.CampId);

            if (camp is null)
            {
                return await Response.FailureAsync("Camp not found.", System.Net.HttpStatusCode.NotFound);
            }

            var requests = await _unitOfWork.Repository<NewRegisteration>().Entities
                      .Where(x => command.RequestsId.Contains(x.Id))
                      .ToListAsync();

            var newAccounts = requests.Adapt<List<Account>>(_mapper.Config);

            foreach (var account in newAccounts)
            {
                account.UserName = _helperService.GetRandomUserNameString(account.FirstName, account.NationalId);
                var password = _helperService.GetRandomPasswordString(account.FirstName, account.NationalId);

                var trainee = new Trainee()
                {
                    CampId = command.CampId
                };

                await _unitOfWork.Trainees.AddAsync(new()
                {
                    Account = account,
                    Member = trainee,
                    Password = password
                });
                await _emailServices.SendAcceptTraineeEmailAsync(account, camp.Name, camp.startDate);
            }

            _unitOfWork.Repository<NewRegisteration>().DeleteRange(requests);

            await _unitOfWork.SaveAsync();

            return await Response.SuccessAsync("Requests accepted.");
        }
    }
}
