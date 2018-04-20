using System;
using System.Windows;

using NoDoze.Bindings;
using NoDoze.Interfaces;
using NoDoze.Logging;

namespace NoDoze
{
    /// <inheritdoc cref="Window"/>
    public partial class MainWindow
    {
        private readonly ILogger _logger = DiFactory.Resolve<ILogger>();
        private readonly NotificationTray _notificationTray;

        public MainWindow()
        {
            _notificationTray = new NotificationTray();

            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            _logger.Log(new LogEntry(LoggingEventType.Debug, "NoDoze::MainWindow::OnInitialized()"));

            Closed += OnClosed;
            Closing += OnClosing;
            Loaded += OnLoaded;
            StateChanged += OnStateChanged;

            base.OnInitialized(e);
        }

        private void OnClosed(object sender, EventArgs e)
        {
            _logger.Log(new LogEntry(LoggingEventType.Debug, "NoDoze::MainWindow::OnClosed()"));
        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _logger.Log(new LogEntry(LoggingEventType.Debug, "NoDoze::MainWindow::OnClosing()"));

            if (WindowState != WindowState.Minimized)
            {
                // Prevent the window from closing (as we'll minimise instead).
                e.Cancel = true;
                
                // Minimise the window to the notification tray
                WindowState = WindowState.Minimized;
            }

        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _logger.Log(new LogEntry(LoggingEventType.Debug, "NoDoze::MainWindow::OnLoaded()"));
        }

        private void OnStateChanged(object sender, EventArgs e)
        {
            _logger.Log(new LogEntry(LoggingEventType.Debug, "NoDoze::MainWindow::OnStateChanged()"));

            if (WindowState != WindowState.Minimized)
                return;

            Visibility = Visibility.Hidden;
           _notificationTray.Visible = true;
        }
    }
}
