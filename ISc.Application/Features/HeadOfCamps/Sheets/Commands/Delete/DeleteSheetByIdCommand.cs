using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace ISc.Application.Features.HeadOfCamps.Sheets.Commands.Delete
{
    public record DeleteSheetByIdCommand : IRequest<Response>
    {
        public int Id { get; set; }

        public DeleteSheetByIdCommand(int id)
        {
            Id = id;
        }
    }

    internal class DeleteSheetByIdCommandHandler : IRequestHandler<DeleteSheetByIdCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<Account> _userManager;

        public DeleteSheetByIdCommandHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor,
            UserManager<Account> userManager)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        public async Task<Response> Handle(DeleteSheetByIdCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext!.User);

            if (user is null)
            {
                return await Response.FailureAsync("Unauthorized.", System.Net.HttpStatusCode.Unauthorized);
            }

            var head = await _unitOfWork.Heads.GetByIdAsync(user.Id);
            var sheet = await _unitOfWork.Repository<Sheet>().GetByIdAsync(command.Id);

            if (sheet is null)
            {
                return await Response.FailureAsync("Sheet not found.", System.Net.HttpStatusCode.NotFound);
            }

            if (head is null || head.CampId != sheet.CampId)
            {
                return await Response.FailureAsync("Unauthorized.", System.Net.HttpStatusCode.Unauthorized);
            }

            _unitOfWork.Repository<Sheet>().Delete(sheet);
            await _unitOfWork.SaveAsync();

            return await Response.SuccessAsync("Sheet deleted.");
        }
    }
}
