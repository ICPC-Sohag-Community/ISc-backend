using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models.CommunityStuff;

namespace ISc.Presistance.Repos
{
    public class HeadRepo : BaseRepo<HeadOfCamp>, IHeadRepo
    {
        public HeadRepo(ICPCDbContext context) : base(context)
        {
        }
        public override void Delete(HeadOfCamp entity)
        {
            base.Delete(entity);
        }
    }
}
