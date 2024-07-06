using ISc.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISc.Presistance.EntitiesConfigurations
{
    internal class TraineeContestAccessConfig : IEntityTypeConfiguration<TraineeAccessContest>
    {
        public void Configure(EntityTypeBuilder<TraineeAccessContest> builder)
        {
            builder.HasKey(x => new { x.TraineeId, x.ContestId, x.Index });

            builder.HasOne(x => x.Contest)
               .WithMany(x => x.Trainees)
               .HasForeignKey(x => x.ContestId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
