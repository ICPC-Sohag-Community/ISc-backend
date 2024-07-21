using Bogus;
using ISc.Domain.Comman.Enums;
using ISc.Domain.Models.IdentityModels;
using System.Runtime.Serialization;

namespace ISc.UnitTests.FakesObjects
{
    internal class FakeAccount : Faker<Account>
    {
        public FakeAccount()
        {
            // Rule for FirstName
            RuleFor(a => a.FirstName, f => f.Name.FirstName());

            // Rule for MiddleName (optional)
            RuleFor(a => a.MiddleName, f => f.Name.FirstName());

            // Rule for LastName
            RuleFor(a => a.LastName, f => f.Name.LastName());

            // Rule for NationalId
            RuleFor(a => a.NationalId, f => f.Random.Long(10000000000000, 99999999999999).ToString());

            // Rule for BirthDate
            RuleFor(a => a.BirthDate,f => f.Date.BetweenDateOnly(DateOnly.MinValue,DateOnly.MaxValue));
            // Rule for CodeForceHandle
            RuleFor(a => a.CodeForceHandle, f => f.Internet.UserName());

            // Rule for College
            RuleFor(a => a.College, f => f.PickRandom<College>());

            // Rule for Email
            RuleFor(a => a.Email, f => f.Internet.Email());

            // Rule for CreationDate
            RuleFor(a => a.CreationDate, f => f.Date.Past());

            // Rule for Gender
            RuleFor(a => a.Gender, f => f.PickRandom<Gender>());

            // Rule for Grade
            RuleFor(a => a.Grade, f => f.Random.Int(1, 5));

            // Rule for PhoneNumber
            RuleFor(a => a.PhoneNumber, f => f.Random.Number(100, 999) + f.Random.Number(1000000, 9999999).ToString());

            // Rule for UserName
            RuleFor(a => a.UserName, f => f.Internet.UserName());
        }
    }
}
