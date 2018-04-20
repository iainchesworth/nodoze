using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

using NoDoze.Bindings;
using NoDoze.Helpers;
using NoDoze.Interfaces;
using NoDoze.Logging;

namespace NoDoze
{
    public class NotificationTray
    {
        private readonly NotifyIconWithClickHandler _notifyIcon;
        private readonly Dictionary<string, Icon> _iconHandles;
        private readonly ISleepingService _sleepingService = DiFactory.Resolve<ISleepingService>();
        private readonly ILogger _logger = DiFactory.Resolve<ILogger>();

        private const string NoDozePermitIcon = "NoDoze_SleepingPermitted";
        private const string NoDozePreventIcon = "NoDoze_SleepingPrevented";

        public NotificationTray()
        {
            _iconHandles = new Dictionary<string, Icon>
            {
                { NoDozePermitIcon, (Icon)Properties.Resources.ResourceManager.GetObject("TrayIcon_DozeEnabled") },
                { NoDozePreventIcon, (Icon)Properties.Resources.ResourceManager.GetObject("TrayIcon_DozeDisabled") }
            };

            _notifyIcon = new NotifyIconWithClickHandler
            {
                Icon = _iconHandles[NoDozePermitIcon],
                Visible = false
            };
            _notifyIcon.MouseClick += NotifyIcon_Click;
            _notifyIcon.MouseDoubleClick += NotifyIcon_DoubleClick;

            _notifyIcon.ContextMenuStrip = new ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.Items.Add(Support.ToolStripMenuItemWithHandler("&Stay Awake", "toggleSleeping", ToggleSleepingItem_Click));
            _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            _notifyIcon.ContextMenuStrip.Items.Add(Support.ToolStripMenuItemWithHandler("&Exit", "exit", ExitItem_Click));

            SwitchToPreventSleeping();
        }

        public bool Visible { get => _notifyIcon.Visible; set =>_notifyIcon.Visible = value; }

        private void NotifyIcon_Click(object sender, MouseEventArgs e)
        {
            _logger.Log(new LogEntry(LoggingEventType.Debug, "NoDoze::NotificationTray::NotifyIcon_Click()"));

            // Given that the NotifyIconWithClickHandler is likely inside a different thread,
            // this needs to be marshalled across the UIThread.

            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                if (e.Button != MouseButtons.Left)
                    return;

                if (_sleepingService.SleepingIsPermitted)
                {
                    SwitchToPreventSleeping();
                }
                else
                {
                    SwitchToPermitSleeping();
                }
            });
        }

        private void NotifyIcon_DoubleClick(object sender, MouseEventArgs e)
        {
            _logger.Log(new LogEntry(LoggingEventType.Debug, "NoDoze::NotificationTray::NotifyIcon_DoubleClick()"));

            // Given that the NotifyIconWithClickHandler is likely inside a different thread,
            // this needs to be marshalled across the UIThread.

            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                var mainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);

                if (mainWindow == null)
                {
                    // Something went wrong and there is no reference to the main window!
                    _logger.Log(new LogEntry(LoggingEventType.Warning, "NoDoze::NotificationTray::NotifyIcon_DoubleClick() - Null reference detected"));
                }
                else if (mainWindow.WindowState != WindowState.Minimized)
                {
                    // If the window is not minimised...we should not be here!
                    _logger.Log(new LogEntry(LoggingEventType.Warning, "NoDoze::NotificationTray::NotifyIcon_DoubleClick() - Not minimized but received a double-click notification"));
                }
                else if (mainWindow.Visibility != Visibility.Hidden)
                {
                    // If the window is not hidden...we should not be here!
                    _logger.Log(new LogEntry(LoggingEventType.Warning, "NoDoze::NotificationTray::NotifyIcon_DoubleClick() - Not hidden but received a double-click notification"));
                }
                else
                {
                    _notifyIcon.Visible = false;

                    // Restore the main window (which should trigger an OnStateChanged event)
                    mainWindow.Show();
                    mainWindow.Activate();
                }
            });
        }

        private void ExitItem_Click(object sender, EventArgs e)
        {
            _logger.Log(new LogEntry(LoggingEventType.Debug, "NoDoze::NotificationTray::ExitItem_Click()"));

            // Terminate the application.  Note that just "closing" the window
            // will trigger the OnClosing event which cancels the application's
            // attempt to close.

            System.Windows.Application.Current.Shutdown();
        }

        private void ToggleSleepingItem_Click(object sender, EventArgs e)
        {
            if (_sleepingService.SleepingIsPermitted)
            {
                SwitchToPreventSleeping();
            }
            else
            {
                SwitchToPermitSleeping();
            }
        }

        private void SwitchToPreventSleeping()
        {
            _logger.Log(new LogEntry(LoggingEventType.Debug, "NoDoze::NotificationTray::SwitchToPreventSleeping()"));
            _logger.Log(new LogEntry(LoggingEventType.Information, "NoDoze - Sleeping INHIBITED"));

            _sleepingService.PreventSleeping();
            _notifyIcon.Icon = _iconHandles[NoDozePreventIcon];
            ((ToolStripMenuItem)_notifyIcon.ContextMenuStrip.Items[0]).Checked = true;  // Check "Stay Awake" menu item
        }

        private void SwitchToPermitSleeping()
        {
            _logger.Log(new LogEntry(LoggingEventType.Debug, "NoDoze::NotificationTray::SwitchToPermitSleeping()"));
            _logger.Log(new LogEntry(LoggingEventType.Information, "NoDoze - Sleeping PERMITTED"));

            _sleepingService.PermitSleeping();
            _notifyIcon.Icon = _iconHandles[NoDozePermitIcon];
            ((ToolStripMenuItem)_notifyIcon.ContextMenuStrip.Items[0]).Checked = false; // Uncheck "Stay Awake" menu item
        }
    }
}
