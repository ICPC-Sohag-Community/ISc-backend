using FluentValidation;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Comman.Enums;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using MapsterMapper;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ISc.Application.Features.Mentors.Practices.Commands.Update
{
    public record UpdatePracticeCommand:IRequest<Response>
    {
        public int PracticeId { get; set; }
        public string Title { get; set; }
        public string MeetingLink { get; set; }
        public string Note { get; set; }
        public DateTime Time { get; set; }
        public PracticeStatus State { get; set; }
    }

    internal class UpdatePracticeCommandHandler : IRequestHandler<UpdatePracticeCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<Account> _userManager;
        private readonly IValidator<UpdatePracticeCommand> _validator;
        private readonly IMapper _mapper;

        public UpdatePracticeCommandHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor,
            UserManager<Account> userManager,
            IValidator<UpdatePracticeCommand> validator,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<Response> Handle(UpdatePracticeCommand command, CancellationToken cancellationToken)
        {
            var validationResult=await _validator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                return await Response.ValidationFailureAsync(validationResult.Errors.ToList(), HttpStatusCode.UnprocessableEntity);
            }

            var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext!.User);

            if (user is null)
            {
                return await Response.FailureAsync("Unauthroized.", HttpStatusCode.Unauthorized);
            }

            var practice = await _unitOfWork.Repository<Practice>().Entities
                         .SingleOrDefaultAsync(x=>x.Id==command.PracticeId&&x.MentorId==user.Id,cancellationToken);

            if (practice is null)
            {
                return await Response.FailureAsync("Practice not found.", HttpStatusCode.NotFound);
            }

            _mapper.Map(command, practice);

            await _unitOfWork.Repository<Practice>().UpdateAsync(practice);
            await _unitOfWork.SaveAsync();

            return await Response.SuccessAsync(practice.Id, "Update practice.");
        }
    }
}
