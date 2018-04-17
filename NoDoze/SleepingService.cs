using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoDoze
{
    public class SleepingService : ISleepingService
    {
        // The default here is to "permit" sleeping as it requires explicit
        // actions by the application to "prevent" sleeping.
        private bool sleepingIsPermitted = true; 

        public bool SleepingIsPermitted
        {
            get { return sleepingIsPermitted; }
        }
        
        public void PermitSleeping()
        {
            NativeMethods.PermitSleep();
            sleepingIsPermitted = true;
        }

        public void PreventSleeping()
        {
            NativeMethods.PreventSleep();
            sleepingIsPermitted = false;
        }
    }
}
