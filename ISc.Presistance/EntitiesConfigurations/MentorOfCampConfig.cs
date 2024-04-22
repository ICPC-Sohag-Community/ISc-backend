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
    internal class MentorOfCampConfig : IEntityTypeConfiguration<MentorsOfCamp>
    {
        public void Configure(EntityTypeBuilder<MentorsOfCamp> builder)
        {
            builder.HasKey(x => new { x.CampId, x.MentorId });

            builder.HasOne(x => x.Mentor)
                .WithMany(x => x.Camps)
                .HasForeignKey(x => x.MentorId);
        }
    }
}
