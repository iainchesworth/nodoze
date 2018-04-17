using Serilog;
using System;
using System.Windows;

namespace NoDoze
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ILogger logger;
        private ISleepingService sleepingService;
        private NotificationTray notificationTray;

        public MainWindow()
        {
            var serilogLogger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            logger = new SerilogAdapter(serilogLogger);

            sleepingService = new SleepingService();
            notificationTray = new NotificationTray(logger, sleepingService);

            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            Closed += OnClosed;
            Closing += OnClosing;
            Loaded += OnLoaded;
            StateChanged += OnStateChanged;

            base.OnInitialized(e);
        }

        private void OnClosed(object sender, System.EventArgs e)
        {
        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
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
        }

        private void OnStateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.Visibility = Visibility.Hidden;
                notificationTray.Visible = true;
            }
        }
    }
}
