using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.Mentors.Practices.Commands.Delete
{
    public record DeletePracticeCommand:IRequest<Response>
    {
        public int PracticeId { get; set; }
    }

    internal class DeletePracticeCommandHandler : IRequestHandler<DeletePracticeCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<Account> _userManager;

        public DeletePracticeCommandHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor,
            UserManager<Account> userManager)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        public async Task<Response> Handle(DeletePracticeCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext!.User);

            var entity = await _unitOfWork.Repository<Practice>().GetByIdAsync(command.PracticeId);

            if(user is null || user.Id != entity.MentorId)
            {
                return await Response.FailureAsync("Unauthroized.", HttpStatusCode.Unauthorized);
            }

            if(entity is null)
            {
                return await Response.FailureAsync("Practice not found.", HttpStatusCode.NotFound);
            }

            _unitOfWork.Repository<Practice>().Delete(entity);
            await _unitOfWork.SaveAsync();

            return await Response.SuccessAsync("Practice deleted.");
        }
    }
}
