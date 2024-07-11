using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace ISc.Application.Features.HeadOfCamps.Sheets.Queries.GetById
{
    public record GetSheetByIdQuery : IRequest<Response>
    {
        public int Id { get; set; }

        public GetSheetByIdQuery(int id)
        {
            Id = id;
        }
    }

    internal class GetSheetByIdQueryHandler : IRequestHandler<GetSheetByIdQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<Account> _userManager;

        public GetSheetByIdQueryHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor,
            UserManager<Account> userManager)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        public async Task<Response> Handle(GetSheetByIdQuery query, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext!.User);

            if (user is null)
            {
                return await Response.FailureAsync("Unauthorized.", System.Net.HttpStatusCode.Unauthorized);
            }

            var head = await _unitOfWork.Heads.GetByIdAsync(user.Id);

            if (head is null)
            {
                return await Response.FailureAsync("Unauthorized.", System.Net.HttpStatusCode.Unauthorized);
            }

            var entity = await _unitOfWork.Repository<Sheet>().GetByIdAsync(query.Id);

            if(entity is null)
            {
                return await Response.FailureAsync("Sheet not found.",System.Net.HttpStatusCode.NotFound);
            }

            if (entity.CampId != head.CampId)
            {
                return await Response.FailureAsync("Unauthorized.", System.Net.HttpStatusCode.Unauthorized);
            }

            var sheet = entity.Adapt<GetSheetByIdQueryDto>();

            return await Response.SuccessAsync(sheet);
        }
    }
}
