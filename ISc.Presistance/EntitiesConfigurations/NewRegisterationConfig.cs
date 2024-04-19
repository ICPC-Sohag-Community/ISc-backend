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
    internal class NewRegisterationConfig : IEntityTypeConfiguration<NewRegisteration>
    {
        public void Configure(EntityTypeBuilder<NewRegisteration> builder)
        {
            throw new NotImplementedException();
        }
    }
}
