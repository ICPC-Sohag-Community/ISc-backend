using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISc.Application.Features.Leader.Camps.Commands.Update;
using ISc.Domain.Models;
using Mapster;

namespace ISc.Application.Comman.Mapping
{
    internal class CampMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<UpdateCampCommand, Camp>();
        }
    }
}
