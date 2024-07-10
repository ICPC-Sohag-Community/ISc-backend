using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISc.Application.Features.Leader.Archives.Queries.GetAllStaffsArchiveWithPagination;
using ISc.Application.Interfaces;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using Mapster;

namespace ISc.Application.Comman.Mapping
{
    public class StaffArchiveMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Account,StaffArchive>();
            config.NewConfig<StaffArchive, GetAllStaffsArchiveWithPaginationQueryDto>();
        }
    }
}
