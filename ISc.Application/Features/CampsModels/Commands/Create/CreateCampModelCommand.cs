using System.Net;
using FluentValidation;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ISc.Application.Features.CampsModels.Commands.Create
{
    public record CreateCampModelCommand : IRequest<Response>
    {
        public string Name { get; set; }

        public CreateCampModelCommand(string name)
        {
            Name = name;
        }
    }

    internal class CreateCampModelCommandHandler : IRequestHandler<CreateCampModelCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateCampModelCommand> _validator;

        public CreateCampModelCommandHandler(
            IUnitOfWork unitOfWork,
            IValidator<CreateCampModelCommand> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<Response> Handle(CreateCampModelCommand query, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(query, cancellationToken);

            if (!validationResult.IsValid)
            {
                return await Response.ValidationFailureAsync(validationResult.Errors.ToList(), HttpStatusCode.UnprocessableEntity);
            }

            if(await _unitOfWork.Repository<CampModel>().Entities.AnyAsync(x => x.Name == query.Name))
            {
                return await Response.FailureAsync("Name already exist.");
            }

            var model = new CampModel() { Name = query.Name };

            await _unitOfWork.Repository<CampModel>().AddAsync(model);
            await _unitOfWork.SaveAsync();

            return await Response.SuccessAsync("Model added successfully");
        }
    }
}
