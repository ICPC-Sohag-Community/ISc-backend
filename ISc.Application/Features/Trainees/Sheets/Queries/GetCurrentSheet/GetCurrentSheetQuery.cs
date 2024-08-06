using ISc.Application.Interfaces.Repos;
using ISc.Domain.Comman.Enums;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.Trainees.Sheets.Queries.GetCurrentSheet
{
    public record GetCurrentSheetQuery:IRequest<Response>;

    internal class GetCurrentSheetQueryHandler : IRequestHandler<GetCurrentSheetQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<Account> _userManager;

        public GetCurrentSheetQueryHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor,
            UserManager<Account> userManager)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        public async Task<Response> Handle(GetCurrentSheetQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext!.User);

            if (user is null)
            {
                return await Response.FailureAsync("Unauthorized.", HttpStatusCode.Unauthorized);
            }

            var trainee = await _unitOfWork.Trainees.GetByIdAsync(user.Id);

            if (trainee is null)
            {
                return await Response.FailureAsync("Unauthorized.", HttpStatusCode.Unauthorized);
            }

            var entity = await _unitOfWork.Repository<Sheet>().Entities
                         .Where(x => x.Status == SheetStatus.InProgress)
                         .OrderBy(x => x.SheetOrder)
                         .FirstOrDefaultAsync();

            var currentSheet = entity.Adapt<GetCurrentSheetQueryDto>();

            return await Response.SuccessAsync(currentSheet);
        }
    }
}
