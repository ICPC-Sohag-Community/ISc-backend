using ISc.Application.Features.HeadOfCamps.Assigning.Queries.GetMentorAssign;
using ISc.Application.Features.HeadOfCamps.Assigning.Queries.GetTraineeAssign;
using ISc.Domain.Models;
using Mapster;

namespace ISc.Application.Comman.Mapping
{
    public class TraineeMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Trainee, GetTraineeAssignQueryDto>()
                .Map(dest => dest, src => src.Account);
            config.NewConfig<Trainee, GetTraineeForMentorAssignDto>()
                .Map(dest => dest, src => src.Account);
        }
    }
}
