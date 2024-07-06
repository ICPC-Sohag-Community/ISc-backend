using ISc.Application.Interfaces.Repos;
using ISc.Domain.Comman.Constant;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ISc.Application.Features.Leader.Camps.Queries.GetAllHeadsOfCamp
{
    public record GetAllHeadsOfCampQuery : IRequest<Response>;
    internal class GetHeadsOfCampQueryHandler : IRequestHandler<GetAllHeadsOfCampQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<Account> _userManager;

        public GetHeadsOfCampQueryHandler(
            IUnitOfWork unitOfWork,
            UserManager<Account> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<Response> Handle(GetAllHeadsOfCampQuery query, CancellationToken cancellationToken)
        {
            var heads = await _unitOfWork.Heads.Entities
                        .Select(m => new GetAllHeadsOfCampQueryDto()
                        {
                            Id = m.Id,
                            FullName = $"{m.Account.FirstName} {m.Account.MiddleName} {m.Account.LastName}",
                            IsInCamp = true
                        }).ToListAsync();

            var mentors = await _unitOfWork.Mentors.Entities
                              .Where(x => !heads.Select(x => x.Id).Contains(x.Id))
                             .Select(m => new GetAllHeadsOfCampQueryDto()
                             {
                                 Id = m.Id,
                                 FullName = $"{m.Account.FirstName} {m.Account.MiddleName} {m.Account.LastName}",
                                 IsInCamp = false
                             })
                             .ToListAsync(cancellationToken);

            var trainees = await _userManager.GetUsersInRoleAsync(Roles.Trainee);
            var stuff = await _userManager.Users.Where(x => !trainees.Select(x => x.Id).Contains(x.Id))
                              .Select(x => new GetAllHeadsOfCampQueryDto()
                              {
                                  Id = x.Id,
                                  FullName = $"{x.FirstName} {x.MiddleName} {x.LastName}",
                                  IsInCamp = false
                              })
                              .ToListAsync(cancellationToken);

            stuff = stuff.Where(x => !mentors.Select(c => c.Id).Contains(x.Id) && !heads.Select(c => c.Id).Contains(x.Id))
                         .ToList();


            return await Response.SuccessAsync(heads.Concat(mentors).Concat(stuff).ToList());
        }
    }
}
