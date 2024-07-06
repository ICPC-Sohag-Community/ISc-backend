using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISc.Presistance.EntitiesConfigurations
{
    internal class TraineeConfig : IEntityTypeConfiguration<Trainee>
    {
        public void Configure(EntityTypeBuilder<Trainee> builder)
        {
            builder.HasOne(x => x.Account)
                .WithOne()
                .HasPrincipalKey<Account>(x => x.Id)
                .HasForeignKey<Trainee>(x => x.Id);

            builder.HasMany(x => x.Tasks)
                .WithOne()
                .HasForeignKey(x => x.TraineeId);

            builder.HasOne(x => x.Mentor)
                .WithMany(x => x.Trainees)
                .HasForeignKey(x => x.MentorId);
        }
    }
}
