using ISc.Application.Interfaces;

namespace ISc.Infrastructure.Services.Time
{
    public class TimeService : ITimeServices
    {
        public DateTime ConvertEgyptTimeZoneToServerZone(DateTime egyptTime)
        {
            TimeZoneInfo serverTimeZone = TimeZoneInfo.Local;

            return TimeZoneInfo.ConvertTimeToUtc(egyptTime.ToUniversalTime(), serverTimeZone);
        }

        public DateTime GetEgyptTimezone(DateTime time)
        {
            time = time.ToUniversalTime();
            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(time, "Egypt Standard Time");
        }
    }
}
