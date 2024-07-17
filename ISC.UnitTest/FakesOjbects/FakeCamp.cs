using Bogus;
using ISc.Domain.Comman.Enums;
using ISc.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.UnitTests.FakesOjbects
{
    internal class FakeCamp:Faker<Camp>
    {
        public FakeCamp()
        {
            RuleFor(x=>x.Name, f=>f.Name.JobTitle());
            RuleFor(x => x.DurationInWeeks, f => f.Random.Int(1, 10));
            RuleFor(x => x.OpenForRegister, true);
            RuleFor(x => x.startDate, DateOnly.MinValue);
            RuleFor(x => x.EndDate, DateOnly.MaxValue);
            RuleFor(x => x.Term, f => f.PickRandom<Term>());
        }
    }
}
