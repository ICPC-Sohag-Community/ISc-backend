using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.HeadOfCamps.Materials.Commands.Delete
{
    public record DeleteMaterialByIdCommand:IRequest<Response>
    {
        public int Id { get; set; }

        public DeleteMaterialByIdCommand(int id)
        {
            Id = id;
        }
    }

    internal class DeleteMaterialByIdCommandHandler : IRequestHandler<DeleteMaterialByIdCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<Account> _userManager;

        public DeleteMaterialByIdCommandHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor,
            UserManager<Account> userManager)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        public async Task<Response> Handle(DeleteMaterialByIdCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext!.User);

            if (user == null)
            {
                return await Response.FailureAsync("Unauthorized.", System.Net.HttpStatusCode.Unauthorized);
            }

            var head = await _unitOfWork.Heads.GetByIdAsync(user.Id);

            if(head is null)
            {
                return await Response.FailureAsync("Unauthorized.", System.Net.HttpStatusCode.Unauthorized);
            }

            var entity = await _unitOfWork.Repository<Material>().GetByIdAsync(command.Id);


            if (entity == null || entity.Sheet.CampId != head.CampId)
            {
                return await Response.FailureAsync("material not found.", System.Net.HttpStatusCode.NotFound);
            }

            var materials = await _unitOfWork.Repository<Material>().Entities
                            .OrderBy(x=>x.MaterialOrder)
                            .ToListAsync();

            int order = 1;
            foreach(var material  in materials)
            {
                if (material.Id == command.Id)
                    continue;

                material.MaterialOrder = order++;
            }

            _unitOfWork.Repository<Material>().UpdateRange(materials);
            _unitOfWork.Repository<Material>().Delete(entity);
            await _unitOfWork.SaveAsync();

            return await Response.SuccessAsync("Material deleted.");
        }
    }
}
