using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ISc.Application.Features.HeadOfCamps.Materials.Queries.GetAllMaterials
{
    public record GetAllMaterialsQuery : IRequest<Response>
    {
        public int SheetId { get; set; }

        public GetAllMaterialsQuery(int sheetId)
        {
            SheetId = sheetId;
        }
    }

    internal class GetAllMaterialsQueryHandler : IRequestHandler<GetAllMaterialsQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<Account> _userManager;
        public GetAllMaterialsQueryHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor,
            UserManager<Account> userManager)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        public async Task<Response> Handle(GetAllMaterialsQuery query, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext!.User);

            if (user == null)
            {
                return await Response.FailureAsync("Unauthorized.", System.Net.HttpStatusCode.Unauthorized);
            }

            var head = await _unitOfWork.Heads.GetByIdAsync(user.Id);

            if (head is null)
            {
                return await Response.FailureAsync("Unauthorized.", System.Net.HttpStatusCode.Unauthorized);
            }

            var sheet = await _unitOfWork.Repository<Sheet>().Entities
                        .SingleOrDefaultAsync(x => x.Id == query.SheetId && x.CampId == head.CampId);

            if (sheet is null)
            {
                return await Response.FailureAsync("Sheet not found.", System.Net.HttpStatusCode.NotFound);
            }

            var materials = sheet.Materials.OrderBy(x => x.MaterialOrder).Adapt<List<GetAllMaterialsQueryDto>>();

            return await Response.SuccessAsync(materials);
        }
    }
}
