using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Interfaces
{
    public interface ITimeServices
    {
        DateTime ConvertEgyptTimeZoneToServerZone(DateTime egyptTime);
        DateTime GetEgyptTimezone(DateTime time);
    }
}
