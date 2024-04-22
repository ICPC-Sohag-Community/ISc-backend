﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISc.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISc.Presistance.EntitiesConfigurations
{
    internal class TraineeArchiveConfig : IEntityTypeConfiguration<TraineeArchive>
    {
        [Obsolete]
        public void Configure(EntityTypeBuilder<TraineeArchive> builder)
        {
            builder.HasKey(x => x.NationalId);

            builder.Property(x => x.FirstName).HasMaxLength(20);
            builder.Property(x => x.MiddelName).HasMaxLength(20);
            builder.Property(x => x.LastName).HasMaxLength(20);
            builder.Property(x => x.NationalId).HasMaxLength(14);
            builder.HasCheckConstraint("GradeConstrain", "Grade between 1 and 5 ");
            builder.HasCheckConstraint("GenderConstarin", "Gender between 0 and 1");
            builder.Property(x => x.VjudgeHandle).HasMaxLength(25);
            builder.Property(x => x.CodeForceHandle).HasMaxLength(25);
            builder.Property(x => x.PhoneNumber).HasMaxLength(12);
        }
    }
}
