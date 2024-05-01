using ISc.Domain.Models.CommunityStuff;
using ISc.Domain.Models.IdentityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISc.Presistance.EntitiesConfigurations
{
    internal class Mentorconfig : IEntityTypeConfiguration<Mentor>
    {
        public void Configure(EntityTypeBuilder<Mentor> builder)
        {
            builder.HasOne(x => x.Account)
                .WithOne()
                .HasPrincipalKey<Account>(x => x.Id)
                .HasForeignKey<Mentor>(x => x.Id);
        }
    }
}
