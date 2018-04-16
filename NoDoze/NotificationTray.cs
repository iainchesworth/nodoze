using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace NoDoze
{
    public class NotificationTray
    {
        private NotifyIcon notifyIcon = null;
        private Dictionary<string, Icon> IconHandles = null;

        public NotificationTray()
        {
            const string NoDoze_IconTitle = "NoDoze";

            IconHandles = new Dictionary<string, Icon>();
            IconHandles.Add(NoDoze_IconTitle, (System.Drawing.Icon)Properties.Resources.ResourceManager.GetObject("TrayIcon_DozeEnabled"));

            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = IconHandles[NoDoze_IconTitle];
            notifyIcon.MouseClick += NotifyIcon_Click;
            notifyIcon.MouseDoubleClick += NotifyIcon_DoubleClick;
            notifyIcon.Visible = false;

            notifyIcon.ContextMenuStrip = new ContextMenuStrip();
            notifyIcon.ContextMenuStrip.Items.Add(Support.ToolStripMenuItemWithHandler("&Exit", "exit", ExitItem_Click));
        }

        public bool Visible
        {
            get
            {
                return notifyIcon.Visible;
            }
            set
            {
                notifyIcon.Visible = value;
            }
        }

        private void NotifyIcon_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ///TODO
            }
        }

        private void NotifyIcon_DoubleClick(object sender, MouseEventArgs e)
        {
            var mainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);

            if (mainWindow.WindowState != WindowState.Minimized)
            {
                // If the window is not minimised...we should not be here!
                System.Diagnostics.Debug.WriteLine("NoDoze::NotifyIcon_DoubleClick() - Not minimized but received a double-click notification!");
            }
            else if (mainWindow.Visibility != Visibility.Hidden)
            {
                // If the window is not hidden...we should not be here!
                System.Diagnostics.Debug.WriteLine("NoDoze::NotifyIcon_DoubleClick() - Not hidden but received a double-click notification!");
            }
            else
            {
                notifyIcon.Visible = false;

                // Restore the main window (which should trigger an OnStateChanged event)
                mainWindow.Show();
                mainWindow.Activate();
            }
        }

        private void ExitItem_Click(object sender, EventArgs e)
        {
            // Terminate the application.  Note that just "closing" the window
            // will trigger the OnClosing event which cancels the application's
            // attempt to close.

            System.Windows.Application.Current.Shutdown();
        }
    }
}
