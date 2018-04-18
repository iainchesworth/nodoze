using System.Drawing;
using System.Timers;
using System.Windows.Forms;

using NoDoze.Interfaces;
using NoDoze.Logging;

namespace NoDoze
{
    class NotifyIconWithClickHandler
    {
        private NotifyIcon notifyIcon;

        public event MouseEventHandler MouseClick = delegate { };
        public event MouseEventHandler MouseDoubleClick = delegate { };

        private MouseEventArgs MouseClick_LastEventArgs;
        private MouseEventArgs MouseDoubleClick_LastEventArgs;

        private ILogger logger = null;

        private System.Timers.Timer clickTimer;
        private int clickCounter;

        public NotifyIconWithClickHandler(ILogger _logger)
        {
            this.logger = _logger;

            clickTimer = new System.Timers.Timer(SystemInformation.DoubleClickTime);
            clickTimer.Elapsed += new ElapsedEventHandler(EvaluateClicks);

            notifyIcon = new NotifyIcon();
            notifyIcon.MouseDown += MouseDown;
        }

        public ContextMenuStrip ContextMenuStrip
        {
            get { return notifyIcon.ContextMenuStrip; }
            set { notifyIcon.ContextMenuStrip = value; }
        }

        public Icon Icon
        {
            get { return notifyIcon.Icon; }
            set { notifyIcon.Icon = value; }
        }

        public bool Visible
        {
            get { return notifyIcon.Visible; }
            set { notifyIcon.Visible = value; }
        }

        private void MouseDown(object sender, MouseEventArgs e)
        {
            logger.Log(new LogEntry(LoggingEventType.Debug, "NoDoze::NotifyIconWithClickHandler::MouseDown()"));

            clickTimer.Stop();

            MouseClick_LastEventArgs = e;
            MouseDoubleClick_LastEventArgs = e;

            clickCounter++;
            clickTimer.Start();
        }

        private void EvaluateClicks(object sender, ElapsedEventArgs e)
        {
            logger.Log(new LogEntry(LoggingEventType.Debug, "NoDoze::NotifyIconWithClickHandler::EvaluateClicks()"));

            clickTimer.Stop();

            if (clickCounter == 0)
            {
                logger.Log(new LogEntry(LoggingEventType.Warning, "NoDoze::NotifyIconWithClickHandler::EvaluateClicks() - [Double] click event but counter indicates zero mouse down events."));
            }
            else if (clickCounter == 1)
            {
                MouseClick(this, MouseClick_LastEventArgs);
            }
            else if (clickCounter == 2)
            {
                MouseDoubleClick(this, MouseDoubleClick_LastEventArgs);
            }
            else
            {
                logger.Log(new LogEntry(LoggingEventType.Warning, "NoDoze::NotifyIconWithClickHandler::EvaluateClicks() - More than two clicks registered during double click period."));
            }

            clickCounter = 0;
        }
    }
}
