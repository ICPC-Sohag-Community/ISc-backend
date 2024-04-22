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
    internal class HeadOfCampConfig : IEntityTypeConfiguration<HeadOfCamp>
    {
        public void Configure(EntityTypeBuilder<HeadOfCamp> builder)
        {
            builder.ToTable("HeadsOfCamps");
        }
    }
}
