using System;
using System.Windows;

using NoDoze.Bindings;
using NoDoze.Interfaces;
using NoDoze.Logging;
using NoDoze.Services;

namespace NoDoze
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ILogger logger = DIFactory.Resolve<ILogger>();
        private NotificationTray notificationTray;

        public MainWindow()
        {
            notificationTray = new NotificationTray();

            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            logger.Log(new LogEntry(LoggingEventType.Debug, "NoDoze::MainWindow::OnInitialized()"));

            Closed += OnClosed;
            Closing += OnClosing;
            Loaded += OnLoaded;
            StateChanged += OnStateChanged;

            base.OnInitialized(e);
        }

        private void OnClosed(object sender, System.EventArgs e)
        {
            logger.Log(new LogEntry(LoggingEventType.Debug, "NoDoze::MainWindow::OnClosed()"));
        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            logger.Log(new LogEntry(LoggingEventType.Debug, "NoDoze::MainWindow::OnClosing()"));

            if (this.WindowState != WindowState.Minimized)
            {
                // Prevent the window from closing (as we'll minimise instead).
                e.Cancel = true;
                
                // Minimise the window to the notification tray
                this.WindowState = WindowState.Minimized;
            }

        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            logger.Log(new LogEntry(LoggingEventType.Debug, "NoDoze::MainWindow::OnLoaded()"));
        }

        private void OnStateChanged(object sender, EventArgs e)
        {
            logger.Log(new LogEntry(LoggingEventType.Debug, "NoDoze::MainWindow::OnStateChanged()"));

            if (this.WindowState == WindowState.Minimized)
            {
                this.Visibility = Visibility.Hidden;
                notificationTray.Visible = true;
            }
        }
    }
}
