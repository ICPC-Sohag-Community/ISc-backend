using FluentValidation;

namespace ISc.Application.Features.Leader.Accounts.Commands.Create
{
    public class CreateAcountCommandValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAcountCommandValidator()
        {
            RuleFor(x => x.NationalId).Length(14);
            RuleFor(x => x.CodeForceHandle).NotEmpty().MaximumLength(30);
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(30);
            RuleFor(x => x.MiddleName).NotEmpty().MaximumLength(30);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(30);
            RuleFor(x => x.Grade).InclusiveBetween(1, 5);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(13);
        }
    }
}