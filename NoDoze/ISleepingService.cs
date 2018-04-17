using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoDoze
{
    public interface ISleepingService
    {
        bool SleepingIsPermitted { get; }

        void PermitSleeping();
        void PreventSleeping();
    }
}