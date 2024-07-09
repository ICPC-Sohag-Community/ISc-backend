using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ISc.Application.Features.HeadOfCamps.Materials.Commands.UpdateMaterialOrder
{
    public record UpdateMaterialOrderCommand : IRequest<Response>
    {
        public int SheetId { get; set; }
        public List<UpdateMaterialOrderCommandDto> Materials { get; set; }
    }
    public class UpdateMaterialOrderCommandDto
    {
        public int MaterialId { get; set; }
        public int Order { get; set; }
    }

    internal class UpdateMaterialOrderCommandHandler : IRequestHandler<UpdateMaterialOrderCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<Account> _userManager;

        public UpdateMaterialOrderCommandHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor,
            UserManager<Account> userManager)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        public async Task<Response> Handle(UpdateMaterialOrderCommand command, CancellationToken cancellationToken)
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

            var sheet = await _unitOfWork.Repository<Sheet>().Entities
                        .SingleOrDefaultAsync(x => x.CampId == head.CampId && x.Id == command.SheetId, cancellationToken);

            if (sheet is null)
            {
                return await Response.FailureAsync("Sheet not found.", System.Net.HttpStatusCode.NotFound);
            }

            var materials = sheet.Materials.ToHashSet();

            if (command.Materials.Any(x => !materials.Select(x => x.Id).Contains(x.MaterialId)))
            {
                return await Response.FailureAsync("Some materials not found.", System.Net.HttpStatusCode.NotFound);
            }

            foreach (var material in materials)
            {
                material.MaterialOrder = command.Materials.First(x => x.MaterialId == material.Id).Order;
            }

            _unitOfWork.Repository<Material>().UpdateRange(materials);
            await _unitOfWork.SaveAsync();

            return await Response.SuccessAsync("Material Order updated");
        }
    }
}
