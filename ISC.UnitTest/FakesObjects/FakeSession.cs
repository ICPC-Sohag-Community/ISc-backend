using Bogus;
using ISc.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.UnitTests.FakesObjects
{
    internal class FakeSession:Faker<Session>
    {
        public FakeSession()
        {
            RuleFor(x => x.Topic, f => f.Company.CompanyName());
            RuleFor(x => x.StartDate, f => f.Date.Past());
            RuleFor(x => x.EndDate, f => f.Date.Future());
            RuleFor(x => x.InstructorName, f => f.Name.FullName());
            RuleFor(x => x.LocationLink, f => f.Name.JobArea());
            RuleFor(x => x.LocationName, f => f.Name.JobTitle());
        }
    }
}
