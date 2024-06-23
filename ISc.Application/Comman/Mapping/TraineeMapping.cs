using ISc.Application.Features.HeadOfCamps.Assigning.Queries.GetTraineeAssignWithPagination;
using ISc.Domain.Models;
using Mapster;

namespace ISc.Application.Comman.Mapping
{
    public class TraineeMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Trainee, GetTraineeAssignWithPaginationQueryDto>()
                .Map(dest => dest.FirstName, src => src.Account.FirstName)
                .Map(dest => dest.MiddleName, src => src.Account.MiddleName)
                .Map(dest => dest.LastName, src => src.Account.LastName)
                .Map(dest => dest, src => src.Account);
        }
    }
}
