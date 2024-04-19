using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISc.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ISc.Presistance.EntitiesConfigurations
{
    internal class TraineeTaskConfig : IEntityTypeConfiguration<TraineeTask>
    {
        public void Configure(EntityTypeBuilder<TraineeTask> builder)
        {
            throw new NotImplementedException();
        }
    }
}
