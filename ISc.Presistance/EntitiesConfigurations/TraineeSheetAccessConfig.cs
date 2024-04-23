using ISc.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISc.Presistance.EntitiesConfigurations
{
    internal class TraineeSheetAccessConfig : IEntityTypeConfiguration<TraineeAccessSheet>
    {
        public void Configure(EntityTypeBuilder<TraineeAccessSheet> builder)
        {
            builder.HasKey(x => new { x.TraineeId, x.SheetId });

            builder.HasOne(x => x.Sheet)
                .WithMany(x => x.TraineesAccess)
                .HasForeignKey(x => x.SheetId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
