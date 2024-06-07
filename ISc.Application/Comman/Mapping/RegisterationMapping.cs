using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISc.Application.Features.Leader.Request.Queries.DisplayAll;
using ISc.Application.Features.Leader.Request.Queries.DisplayById;
using ISc.Domain.Models;
using Mapster;

namespace ISc.Application.Comman.Mapping
{
    internal class RegisterationMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<NewRegisteration, DisplayAllRegisterationQueryDto>()
                .Map(dest => dest.FullName, src => src.FirstName + ' ' + src.MiddelName + ' ' + src.LastName);
            config.NewConfig<NewRegisteration, DisplayRegisterationByIdQueryDto>()
                .Map(dest => dest.FullName, src => src.FirstName + ' ' + src.MiddelName + ' ' + src.LastName);
        }
    }
}
