using ISc.Application.Extension;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.HeadOfCamps.Contests.Queries.GetAllWithPagination
{
    public record GetAllContestWithPagination:PaginatedRequest,IRequest<PaginatedRespnose<GetAllContestWithPaginationDto>>;

    internal class GetAllContestWithPaginationHandler : IRequestHandler<GetAllContestWithPagination, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<Account> _userManager;

        public GetAllContestWithPaginationHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor,
            UserManager<Account> userManager)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        public async Task<Response> Handle(GetAllContestWithPagination query, CancellationToken cancellationToken)
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

            var entities = _unitOfWork.Repository<Contest>().Entities
                                       .Where(x=>x.CampId==head.CampId)
                                        .OrderBy(x => x.StartTime);

            return await entities.ProjectToType<GetAllContestWithPaginationDto>()
                                .ToPaginatedListAsync(query.PageNumber, query.PageSize, cancellationToken);
        }
    }
}
