using ISc.Domain.Models;
using ISc.Domain.Models.CommunityStuff;
using ISc.Domain.Models.IdentityModels;
using ISc.Presistance.EntitiesConfigurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ISc.Presistance
{
    public class ICPCDbContext : IdentityDbContext<Account>
    {
        public ICPCDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Account>().ToTable("Users", "Account");
            builder.Entity<IdentityRole>().ToTable("Roles", "Account");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "Account");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "Account");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "Account");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "Account");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "Account");

            builder.ApplyConfigurationsFromAssembly(typeof(TraineeConfig).Assembly);

            builder.HasDefaultSchema("ICPC");
        }

        #region Actors
        public virtual DbSet<Trainee> Trainees { get; set; }
        public virtual DbSet<HeadOfCamp> HeadsOfCamps { get; set; }
        #endregion

        #region Data
        public virtual DbSet<TraineeArchive> TraineesArchives { get; set; }
        public virtual DbSet<Camp> Camps { get; set; }
        public virtual DbSet<Material> Materials { get; set; }
        public virtual DbSet<MentorsOfCamp> MentorsOfCamps { get; set; }
        public virtual DbSet<NewRegisteration> NewRegisterations { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<SessionFeedback> SessionFeedbacks { get; set; }
        public virtual DbSet<Sheet> Sheets { get; set; }
        public virtual DbSet<StuffArchive> StuffArchives { get; set; }
        public virtual DbSet<TraineeAttendence> TraineeAttences { get; set; }
        public virtual DbSet<TraineeTask> TraineeTasks { get; set; }
        #endregion
    }
}
