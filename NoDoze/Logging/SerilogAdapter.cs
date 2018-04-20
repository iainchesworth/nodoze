using NoDoze.Interfaces;

namespace NoDoze.Logging

{
    public class SerilogAdapter : ILogger
    {
        private readonly Serilog.ILogger _adaptee;

        public SerilogAdapter(Serilog.ILogger adaptee)
        {
            _adaptee = adaptee;
        }

        public void Log(LogEntry entry)
        {
            switch (entry.Severity)
            {
                case LoggingEventType.Debug:
                    _adaptee.Debug(entry.Exception, entry.Message);
                    break;

                case LoggingEventType.Information:
                    _adaptee.Information(entry.Exception, entry.Message);
                    break;

                case LoggingEventType.Warning:
                    _adaptee.Warning(entry.Message, entry.Exception);
                    break;

                case LoggingEventType.Error:
                    _adaptee.Error(entry.Message, entry.Exception);
                    break;

                // case LoggingEventType.Fatal:
                default:
                    _adaptee.Fatal(entry.Message, entry.Exception);
                    break;
            }
        }
    }
}
