using ISc.Application.Features.Trainees.Contests.Queries.GetAllContests;
using ISc.Domain.Models;
using Mapster;

namespace ISc.Application.Comman.Mapping
{
    internal class ContestMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Contest, GetAllContestsQueryDto>()
                .Map(dest => dest.Date, src => src.StartTime >= DateTime.Now ? src.StartTime : src.EndTime);

            config.NewConfig<Contest, GetAllContestsQueryDto>()
                .Map(dest => dest.IsComming, src => src.StartTime >= DateTime.Now);
        }
    }
}