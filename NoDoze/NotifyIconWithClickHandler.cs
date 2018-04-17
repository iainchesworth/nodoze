using System.Drawing;
using System.Timers;
using System.Windows.Forms;

namespace NoDoze
{
    class NotifyIconWithClickHandler
    {
        private NotifyIcon notifyIcon;

        public event MouseEventHandler MouseClick = delegate { };
        public event MouseEventHandler MouseDoubleClick = delegate { };

        private MouseEventArgs MouseClick_LastEventArgs;
        private MouseEventArgs MouseDoubleClick_LastEventArgs;
        
        private System.Timers.Timer clickTimer;
        private int clickCounter;

        public NotifyIconWithClickHandler()
        {
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
            clickTimer.Stop();

            MouseClick_LastEventArgs = e;
            MouseDoubleClick_LastEventArgs = e;

            clickCounter++;
            clickTimer.Start();
        }

        private void EvaluateClicks(object sender, ElapsedEventArgs e)
        {
            clickTimer.Stop();

            if (clickCounter == 0)
            {
                ///TODO - not correct
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
                ///TODO - handle more than one or two clicks
            }

            clickCounter = 0;
        }
    }
}
