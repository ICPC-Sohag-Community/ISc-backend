using ISc.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISc.Presistance.EntitiesConfigurations
{
    internal class TraineeTaskConfig : IEntityTypeConfiguration<TraineeTask>
    {
        public void Configure(EntityTypeBuilder<TraineeTask> builder)
        {

        }
    }
}
