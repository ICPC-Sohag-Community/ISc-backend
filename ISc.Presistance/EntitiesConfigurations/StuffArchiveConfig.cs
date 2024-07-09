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
    internal class StuffArchiveConfig : IEntityTypeConfiguration<StuffArchive>
    {
        [Obsolete]
        public void Configure(EntityTypeBuilder<StuffArchive> builder)
        {
            builder.HasIndex(x => new { x.NationalId,x.Role}).IsUnique();

            builder.Property(x => x.FirstName).HasMaxLength(30);
            builder.Property(x => x.MiddleName).HasMaxLength(30);
            builder.Property(x => x.LastName).HasMaxLength(30);
            builder.Property(x => x.NationalId).HasMaxLength(14);
            builder.HasCheckConstraint("GradeConstrain", "Grade between 1 and 5 ");
            builder.HasCheckConstraint("GenderConstarin", "Gender between 0 and 1");
            builder.Property(x => x.VjudgeHandle).HasMaxLength(25);
            builder.Property(x => x.CodeForceHandle).HasMaxLength(25);
            builder.Property(x => x.PhoneNumber).HasMaxLength(12);
        }
    }
}
