using ISc.Domain.Models.CommunityStaff;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ISC.UnitTests
{
    public class TestBase
    {
        [Fact]
        public void TestInheritance()
        {
            var Mentor = new Mentor();

            // bool Action = UserMangerRepo.IsAvailableType(Mentor);

            // Assert.False(Action);
        }
    }
}
