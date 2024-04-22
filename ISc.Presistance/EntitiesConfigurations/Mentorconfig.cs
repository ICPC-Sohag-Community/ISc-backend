using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISc.Domain.Models.CommunityStuff;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISc.Presistance.EntitiesConfigurations
{
    internal class Mentorconfig : IEntityTypeConfiguration<Mentor>
    {
        public void Configure(EntityTypeBuilder<Mentor> builder)
        {
            builder.ToTable("Mentors");
        }
    }
}
