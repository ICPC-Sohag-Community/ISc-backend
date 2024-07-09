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
    internal class SessionFeedbackConfig : IEntityTypeConfiguration<SessionFeedback>
    {
        [Obsolete]
        public void Configure(EntityTypeBuilder<SessionFeedback> builder)
        {

            builder.HasKey(x => new { x.SessionId, x.TraineeId });

            builder.HasCheckConstraint("Rate Constrain", "Rate between 1 and 5");

            builder.HasOne(x=>x.Session)
                .WithMany(x=>x.Feedbacks)
                .HasForeignKey(x=>x.SessionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
