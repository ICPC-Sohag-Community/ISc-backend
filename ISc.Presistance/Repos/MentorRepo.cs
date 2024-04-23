using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Domain.Models.CommunityStuff;
using ISc.Domain.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;

namespace ISc.Presistance.Repos
{
    public class MentorRepo : BaseRepo<Mentor>, IMentorRepo
    {
        private readonly ICPCDbContext _context;
        private readonly UserManager<Account> _userManager;
		public MentorRepo(ICPCDbContext context, UserManager<Account> userManager) : base(context)
		{
			_context = context;
			_userManager = userManager;
		}
		public override void Delete(Mentor entity)
        {
            var rolesCount = _userManager.GetRolesAsync(entity).Result.Count();
            if(rolesCount==1)
            {
                base.Delete(entity);
            }    
            else
            {
                DeleteMentorTrainees(  entity);
                _context.MentorsOfCamps.RemoveRange(_context.MentorsOfCamps.Where(p => p.MentorId == entity.Id));
                _userManager.RemoveFromRoleAsync(entity, "");//TODO: Remove with RoleName 
            }
            //TODO: Add To Stuff Archive
        }
   
        private void DeleteMentorTrainees(Mentor entity)
        {
			var trainees = _context.Trainees.Where(p => p.MentorId == entity.Id);
			foreach (var trainee in trainees)
                trainee.MentorId = null;
        }

    }
}
