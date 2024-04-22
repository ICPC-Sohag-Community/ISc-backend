using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models.CommunityStuff;

namespace ISc.Presistance.Repos
{
    public class MentorRepo : BaseRepo<Mentor>, IMentorRepo
    {
        public MentorRepo(ICPCDbContext context) : base(context)
        {
            
        }
        public override void Delete(Mentor entity)
        {
            base.Delete(entity);
        }
    }
}
