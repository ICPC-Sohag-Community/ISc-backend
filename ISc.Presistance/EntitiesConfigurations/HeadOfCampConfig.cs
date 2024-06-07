using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISc.Domain.Models.CommunityStuff;
using ISc.Domain.Models.IdentityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISc.Presistance.EntitiesConfigurations
{
    internal class HeadOfCampConfig : IEntityTypeConfiguration<HeadOfCamp>
    {
        public void Configure(EntityTypeBuilder<HeadOfCamp> builder)
        {
            builder.HasOne(x => x.Account)
                .WithOne()
                .HasPrincipalKey<Account>(x => x.Id)
                .HasForeignKey<HeadOfCamp>(x => x.Id);
        }
    }
}
