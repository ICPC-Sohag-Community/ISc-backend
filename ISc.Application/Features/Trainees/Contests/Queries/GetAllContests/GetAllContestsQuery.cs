using ISc.Application.Interfaces.Repos;
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

namespace ISc.Application.Features.Trainees.Contests.Queries.GetAllContests
{
    public record GetAllContestsQuery:IRequest<Response>;

    internal class GetAllContestsQueryHandler : IRequestHandler<GetAllContestsQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<Account> _userManager;

        public GetAllContestsQueryHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor,
            UserManager<Account> userManager)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        public async Task<Response> Handle(GetAllContestsQuery request, CancellationToken cancellationToken)
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


            var entites = await _unitOfWork.Repository<Contest>().Entities
                        .Where(x => x.CampId == trainee.CampId)
                        .OrderBy(x=>x.StartTime)
                        .ToListAsync(cancellationToken);

            var contests = entites.Adapt<List<GetAllContestsQueryDto>>();

            return await Response.SuccessAsync(contests);
        }
    }
}