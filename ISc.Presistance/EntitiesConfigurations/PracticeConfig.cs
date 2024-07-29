using ISc.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Presistance.EntitiesConfigurations
{
    internal class PracticeConfig : IEntityTypeConfiguration<Practice>
    {
        public void Configure(EntityTypeBuilder<Practice> builder)
        {
            builder.Property(x => x.State).IsRequired();
            builder.Property(x => x.Time).IsRequired();
            builder.Property(x => x.MeetingLink).IsRequired();
            builder.Property(x => x.Note).IsRequired(false);
        }
    }
}
