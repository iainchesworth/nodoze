using System.Drawing;
using System.Timers;
using System.Windows.Forms;

using NoDoze.Bindings;
using NoDoze.Interfaces;
using NoDoze.Logging;

namespace NoDoze
{
    class NotifyIconWithClickHandler
    {
        private readonly NotifyIcon _notifyIcon;

        public event MouseEventHandler MouseClick = delegate { };
        public event MouseEventHandler MouseDoubleClick = delegate { };

        private MouseEventArgs _mouseClickLastEventArgs;
        private MouseEventArgs _mouseDoubleClickLastEventArgs;

        private readonly ILogger _logger = DiFactory.Resolve<ILogger>();

        private readonly System.Timers.Timer _clickTimer;
        private int _clickCounter;

        public NotifyIconWithClickHandler()
        {
            _clickTimer = new System.Timers.Timer(SystemInformation.DoubleClickTime);
            _clickTimer.Elapsed += EvaluateClicks;

            _notifyIcon = new NotifyIcon();
            _notifyIcon.MouseDown += MouseDown;
        }

        public ContextMenuStrip ContextMenuStrip
        {
            get => _notifyIcon.ContextMenuStrip;
            set => _notifyIcon.ContextMenuStrip = value;
        }

        public Icon Icon
        {
            get => _notifyIcon.Icon;
            set => _notifyIcon.Icon = value;
        }

        public bool Visible
        {
            get => _notifyIcon.Visible;
            set => _notifyIcon.Visible = value;
        }

        private void MouseDown(object sender, MouseEventArgs e)
        {
            _logger.Log(new LogEntry(LoggingEventType.Debug, "NoDoze::NotifyIconWithClickHandler::MouseDown()"));

            _clickTimer.Stop();

            _mouseClickLastEventArgs = e;
            _mouseDoubleClickLastEventArgs = e;

            _clickCounter++;
            _clickTimer.Start();
        }

        private void EvaluateClicks(object sender, ElapsedEventArgs e)
        {
            _logger.Log(new LogEntry(LoggingEventType.Debug, "NoDoze::NotifyIconWithClickHandler::EvaluateClicks()"));

            _clickTimer.Stop();

            if (_clickCounter == 0)
            {
                _logger.Log(new LogEntry(LoggingEventType.Warning, "NoDoze::NotifyIconWithClickHandler::EvaluateClicks() - [Double] click event but counter indicates zero mouse down events."));
            }
            else if (_clickCounter == 1)
            {
                MouseClick(this, _mouseClickLastEventArgs);
            }
            else if (_clickCounter == 2)
            {
                MouseDoubleClick(this, _mouseDoubleClickLastEventArgs);
            }
            else
            {
                _logger.Log(new LogEntry(LoggingEventType.Warning, "NoDoze::NotifyIconWithClickHandler::EvaluateClicks() - More than two clicks registered during double click period."));
            }

            _clickCounter = 0;
        }
    }
}
