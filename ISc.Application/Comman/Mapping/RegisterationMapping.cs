using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using Mapster;

namespace ISc.Application.Comman.Mapping
{
    internal class RegisterationMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<NewRegisteration, Account>()
                .Ignore(x => x.Id);
        }
    }
}
