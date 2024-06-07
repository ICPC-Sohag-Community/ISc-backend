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
                .Map(dest => dest.FullName, src => src.Account.FirstName + ' ' + src.Account.MiddleName + ' ' + src.Account.LastName)
                .Map(dest => dest, src => src.Account);
        }
    }
}
