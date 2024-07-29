﻿using ISc.Domain.Interface;
using ISc.Domain.Models;
using ISc.Domain.Models.CommunityStaff;
using ISc.Domain.Models.IdentityModels;
using ISc.Presistance.EntitiesConfigurations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ISc.Presistance
{
    public class ICPCDbContext : IdentityDbContext<Account>
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public ICPCDbContext(
            DbContextOptions options,
            IHttpContextAccessor contextAccessor) : base(options)
        {
            _contextAccessor = contextAccessor;
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
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var claim = _contextAccessor.HttpContext is null || _contextAccessor.HttpContext.User is null ?
                null : _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            foreach (var entity in ChangeTracker
                .Entries()
                .Where(x => x.Entity is IAuditable && x.State == EntityState.Added)
                .Select(x => x.Entity)
                .Cast<IAuditable>())
            {
                entity.CreationDate = DateTime.Now;
            }

            if(claim is not null)
            {
                foreach (var entity in ChangeTracker
                .Entries()
                .Where(x => x.Entity is Auditable && x.State == EntityState.Added)
                .Select(x => x.Entity)
                .Cast<Auditable>())
                {
                    entity.CreatedBy = claim.Value;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        #region Actors
        public virtual DbSet<Trainee> Trainees { get; set; }
        public virtual DbSet<HeadOfCamp> HeadsOfCamps { get; set; }
        public virtual DbSet<Mentor> Mentors { get; set; }
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
        public virtual DbSet<Contest> Contests { get; set; }
        public virtual DbSet<StaffArchive> StuffArchives { get; set; }
        public virtual DbSet<TraineeAttendence> TraineeAttendences { get; set; }
        public virtual DbSet<TraineeAccessSheet> TraineesAccesses { get; set; }
        public virtual DbSet<TraineeAccessContest>TraineesContest { get; set; }
        public virtual DbSet<TraineeTask> TraineeTasks { get; set; }
        public virtual DbSet<CampModel> CampModels { get; set; }
        #endregion
    }
}
