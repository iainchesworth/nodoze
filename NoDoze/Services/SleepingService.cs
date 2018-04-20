using NoDoze.Bindings;
using NoDoze.Helpers;
using NoDoze.Interfaces;
using NoDoze.Logging;

namespace NoDoze.Services
{
    public class SleepingService : ISleepingService
    {
        private readonly ILogger _logger = DiFactory.Resolve<ILogger>();

        // The default here is to "permit" sleeping as it requires explicit
        // actions by the application to "prevent" sleeping.

        // The default here is to "permit" sleeping as it requires explicit
        // actions by the application to "prevent" sleeping.

        public bool SleepingIsPermitted { get; private set; } = true;

        public void PermitSleeping()
        {
            _logger.Log(new LogEntry(LoggingEventType.Debug, "NoDoze::SleepingService::PermitSleeping()"));
            NativeMethods.PermitSleep();
            SleepingIsPermitted = true;
        }

        public void PreventSleeping()
        {
            _logger.Log(new LogEntry(LoggingEventType.Debug, "NoDoze::SleepingService::PreventSleeping()"));
            NativeMethods.PreventSleep();
            SleepingIsPermitted = false;
        }
    }
}
