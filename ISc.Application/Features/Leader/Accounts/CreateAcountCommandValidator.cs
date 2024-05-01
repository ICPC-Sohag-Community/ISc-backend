using FluentValidation;

namespace ISc.Application.Features.Leader.User
{
    public class CreateAcountCommandValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAcountCommandValidator()
        {
            RuleFor(x=>x.NationalId).Length(14);
            RuleFor(x => x.CodeForceHandle).NotEmpty();
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(20);
            RuleFor(x => x.MiddleName).NotEmpty().MaximumLength(20);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(20);
            RuleFor(x => x.Grade).InclusiveBetween(1, 5);
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(13);
        }
    }
}