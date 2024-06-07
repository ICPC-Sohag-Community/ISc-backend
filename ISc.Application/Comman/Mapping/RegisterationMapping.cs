using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISc.Application.Features.Leader.Request.Queries.DisplayAll;
using ISc.Application.Features.Leader.Request.Queries.DisplayById;
using ISc.Application.Features.Leader.Request.Queries.DisplayOnCustomerFilter;
using ISc.Application.Features.Leader.Request.Queries.DisplayOnSystemFilter;
using ISc.Domain.Models;
using Mapster;

namespace ISc.Application.Comman.Mapping
{
    internal class RegisterationMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<NewRegisteration, GetAllRegisterationQueryDto>()
                .Map(dest => dest.FullName, src => src.FirstName + ' ' + src.MiddleName + ' ' + src.LastName);
            config.NewConfig<NewRegisteration, GetRegisterationByIdQueryDto>()
                .Map(dest => dest.FullName, src => src.FirstName + ' ' + src.MiddleName + ' ' + src.LastName);
            config.NewConfig<NewRegisteration, GetRegisterationOnSystemFilterQueryDto>()
                .Map(dest => dest.FullName, src => src.FirstName + ' ' + src.MiddleName + ' ' + src.LastName);
            config.NewConfig<NewRegisteration, GetOnCustomerFilterQueryDto>()
                .Map(dest => dest.FullName, src => src.FirstName + ' ' + src.MiddleName + ' ' + src.LastName);

        }
    }
}
