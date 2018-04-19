using NoDoze.Bindings;
using NoDoze.Helpers;
using NoDoze.Interfaces;
using NoDoze.Logging;

namespace NoDoze.Services
{
    public class SleepingService : ISleepingService
    {
        private ILogger logger = DIFactory.Resolve<ILogger>();

        // The default here is to "permit" sleeping as it requires explicit
        // actions by the application to "prevent" sleeping.

        // The default here is to "permit" sleeping as it requires explicit
        // actions by the application to "prevent" sleeping.

        public bool SleepingIsPermitted { get; private set; } = true;

        public void PermitSleeping()
        {
            logger.Log(new LogEntry(LoggingEventType.Debug, "NoDoze::SleepingService::PermitSleeping()"));
            NativeMethods.PermitSleep();
            SleepingIsPermitted = true;
        }

        public void PreventSleeping()
        {
            logger.Log(new LogEntry(LoggingEventType.Debug, "NoDoze::SleepingService::PreventSleeping()"));
            NativeMethods.PreventSleep();
            SleepingIsPermitted = false;
        }
    }
}
