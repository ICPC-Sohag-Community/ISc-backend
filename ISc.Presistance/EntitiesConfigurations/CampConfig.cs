using ISc.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISc.Presistance.EntitiesConfigurations
{
    internal class CampConfig : IEntityTypeConfiguration<Camp>
    {
        public void Configure(EntityTypeBuilder<Camp> builder)
        {

        }
    }
}
