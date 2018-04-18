﻿using NoDoze.Bindings;
using NoDoze.Helpers;
using NoDoze.Interfaces;

namespace NoDoze.Services
{
    public class SleepingService : ISleepingService
    {
        private ILogger logger = DIFactory.Resolve<ILogger>();
        
        // The default here is to "permit" sleeping as it requires explicit
        // actions by the application to "prevent" sleeping.

        public bool SleepingIsPermitted { get; private set; } = true;
        
        public void PermitSleeping()
        {
            NativeMethods.PermitSleep();
            SleepingIsPermitted = true;
        }

        public void PreventSleeping()
        {
            NativeMethods.PreventSleep();
            SleepingIsPermitted = false;
        }
    }
}
