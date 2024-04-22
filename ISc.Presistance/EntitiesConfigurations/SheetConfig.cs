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
    internal class SheetConfig : IEntityTypeConfiguration<Sheet>
    {
        public void Configure(EntityTypeBuilder<Sheet> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(50);
            builder.Property(x => x.MinimumPassingPrecent).HasMaxLength(100);
        }
    }
}
