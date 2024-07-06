using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISc.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISc.Presistance.EntitiesConfigurations
{
    internal class TraineeAttendenceConfig : IEntityTypeConfiguration<TraineeAttendence>
    {
        public void Configure(EntityTypeBuilder<TraineeAttendence> builder)
        {
            builder.HasKey(x => new { x.TraineeId, x.SessionId });

            builder.HasOne(x => x.Trainee)
                .WithMany(x => x.Attendences)
                .HasForeignKey(x => x.TraineeId);

            builder.HasOne(x=>x.Session)
                .WithMany(x=>x.Attendences)
                .HasForeignKey(x => x.SessionId)
                .OnDelete(deleteBehavior:DeleteBehavior.Restrict);
        }
    }
}
