﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

using NoDoze.Helpers;
using NoDoze.Interfaces;
using NoDoze.Logging;

namespace NoDoze
{
    public class NotificationTray
    {
        private NotifyIconWithClickHandler notifyIcon = null;
        private Dictionary<string, Icon> iconHandles = null;
        private ISleepingService sleepingService = null;
        private ILogger logger = null;

        private const string NoDoze_PermitIcon = "NoDoze_SleepingPermitted";
        private const string NoDoze_PreventIcon = "NoDoze_SleepingPrevented";

        public NotificationTray(ILogger _logger, ISleepingService _sleepingService)
        {
            this.sleepingService = _sleepingService;
            this.logger = _logger;

            iconHandles = new Dictionary<string, Icon>();
            iconHandles.Add(NoDoze_PermitIcon, (System.Drawing.Icon)Properties.Resources.ResourceManager.GetObject("TrayIcon_DozeEnabled"));
            iconHandles.Add(NoDoze_PreventIcon, (System.Drawing.Icon)Properties.Resources.ResourceManager.GetObject("TrayIcon_DozeDisabled"));

            notifyIcon = new NotifyIconWithClickHandler(_logger);
            notifyIcon.Icon = iconHandles[NoDoze_PermitIcon];
            notifyIcon.Visible = false;
            notifyIcon.MouseClick += NotifyIcon_Click;
            notifyIcon.MouseDoubleClick += NotifyIcon_DoubleClick;

            notifyIcon.ContextMenuStrip = new ContextMenuStrip();
            notifyIcon.ContextMenuStrip.Items.Add(Support.ToolStripMenuItemWithHandler("&Stay Awake", "toggleSleeping", ToggleSleepingItem_Click));
            notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            notifyIcon.ContextMenuStrip.Items.Add(Support.ToolStripMenuItemWithHandler("&Exit", "exit", ExitItem_Click));

            SwitchToPreventSleeping();
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
            logger.Log(new LogEntry(LoggingEventType.Debug, "NoDoze::NotificationTray::NotifyIcon_Click()"));

            // Given that the NotifyIconWithClickHandler is likely inside a different thread,
            // this needs to be marshalled across the UIThread.

            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (sleepingService.SleepingIsPermitted)
                    {
                        SwitchToPreventSleeping();
                    }
                    else
                    {
                        SwitchToPermitSleeping();
                    }
                }
            });
        }

        private void NotifyIcon_DoubleClick(object sender, MouseEventArgs e)
        {
            logger.Log(new LogEntry(LoggingEventType.Debug, "NoDoze::NotificationTray::NotifyIcon_DoubleClick()"));

            // Given that the NotifyIconWithClickHandler is likely inside a different thread,
            // this needs to be marshalled across the UIThread.

            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                var mainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);

                if (mainWindow.WindowState != WindowState.Minimized)
                {
                    // If the window is not minimised...we should not be here!
                    logger.Log(new LogEntry(LoggingEventType.Warning, "NoDoze::NotificationTray::NotifyIcon_DoubleClick() - Not minimized but received a double-click notification"));
                }
                else if (mainWindow.Visibility != Visibility.Hidden)
                {
                    // If the window is not hidden...we should not be here!
                    logger.Log(new LogEntry(LoggingEventType.Warning, "NoDoze::NotificationTray::NotifyIcon_DoubleClick() - Not hidden but received a double-click notification"));
                }
                else
                {
                    notifyIcon.Visible = false;

                    // Restore the main window (which should trigger an OnStateChanged event)
                    mainWindow.Show();
                    mainWindow.Activate();
                }
            });
        }

        private void ExitItem_Click(object sender, EventArgs e)
        {
            logger.Log(new LogEntry(LoggingEventType.Debug, "NoDoze::NotificationTray::ExitItem_Click()"));

            // Terminate the application.  Note that just "closing" the window
            // will trigger the OnClosing event which cancels the application's
            // attempt to close.

            System.Windows.Application.Current.Shutdown();
        }

        private void ToggleSleepingItem_Click(object sender, EventArgs e)
        {
            if (sleepingService.SleepingIsPermitted)
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
            logger.Log(new LogEntry(LoggingEventType.Debug, "NoDoze::NotificationTray::SwitchToPreventSleeping()"));
            logger.Log(new LogEntry(LoggingEventType.Information, "NoDoze - Sleeping INHIBITED"));

            sleepingService.PreventSleeping();
            notifyIcon.Icon = iconHandles[NoDoze_PreventIcon];
            ((ToolStripMenuItem)notifyIcon.ContextMenuStrip.Items[0]).Checked = true;  // Check "Stay Awake" menu item
        }

        private void SwitchToPermitSleeping()
        {
            logger.Log(new LogEntry(LoggingEventType.Debug, "NoDoze::NotificationTray::SwitchToPermitSleeping()"));
            logger.Log(new LogEntry(LoggingEventType.Information, "NoDoze - Sleeping PERMITTED"));

            sleepingService.PermitSleeping();
            notifyIcon.Icon = iconHandles[NoDoze_PermitIcon];
            ((ToolStripMenuItem)notifyIcon.ContextMenuStrip.Items[0]).Checked = false; // Uncheck "Stay Awake" menu item
        }
    }
}
