using ISc.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Interfaces
{
    public interface IJobServices
    {
        void TrackingTraineesSolving();
        void TrackingContest(Sheet contest);
    }
}
