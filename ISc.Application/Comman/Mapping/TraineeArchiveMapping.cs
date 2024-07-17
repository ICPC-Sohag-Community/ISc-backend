using ISc.Application.Features.Leader.Archives.Queries.GetAllTraineesArchiveWithPagination;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Comman.Mapping
{
    public class TraineeArchiveMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Account, TraineeArchive>()
                .Ignore(x=>x.Id);
        }
    }
}
