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
    internal class SessionFeedbackConfig : IEntityTypeConfiguration<SessionFeedback>
    {
        public void Configure(EntityTypeBuilder<SessionFeedback> builder)
        {
            throw new NotImplementedException();
        }
    }
}
