namespace NoDoze
{
    public class SerilogAdapter : ILogger
    {
        private readonly Serilog.ILogger m_Adaptee;

        public SerilogAdapter(Serilog.ILogger adaptee)
        {
            m_Adaptee = adaptee;
        }

        public void Log(LogEntry entry)
        {
            switch (entry.Severity)
            {
                case LoggingEventType.Debug:
                    m_Adaptee.Debug(entry.Exception, entry.Message);
                    break;

                case LoggingEventType.Information:
                    m_Adaptee.Information(entry.Exception, entry.Message);
                    break;

                case LoggingEventType.Warning:
                    m_Adaptee.Warning(entry.Message, entry.Exception);
                    break;

                case LoggingEventType.Error:
                    m_Adaptee.Error(entry.Message, entry.Exception);
                    break;

                case LoggingEventType.Fatal:
                default:
                    m_Adaptee.Fatal(entry.Message, entry.Exception);
                    break;
            }
        }
    }
}
