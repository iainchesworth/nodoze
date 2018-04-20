using System.Windows.Input;

using NoDoze.Bindings;
using NoDoze.Helpers;
using NoDoze.Interfaces;
using NoDoze.Logging;

namespace NoDoze.ViewModels
{
    public class MainWindow : ViewModelBase
    {
        private readonly ISleepingService _sleepingService = DiFactory.Resolve<ISleepingService>();
        private readonly ILogger _logger = DiFactory.Resolve<ILogger>();

        private const string StayAwakeSleepingPermittedStatus = "NoDoze Inactive - Sleep Enabled";
        private const string StayAwakeSleepingPreventedStatus = "NoDoze Active - Sleep Disabled";
        private bool _stayAwake;

        public MainWindow()
        {
            // The default here is to prevent sleeping as it requires explicit
            // actions by the user to permit sleeping....after all, it's the 
            // point of the application.

            StayAwake = true;
        }

        private ICommand _exitCommand;
        public ICommand ExitCommand => _exitCommand ?? (_exitCommand = new CommandHandler(() => ExitAction(), true));

        public void ExitAction()
        {
            _logger.Log(new LogEntry(LoggingEventType.Debug, "NoDoze::MainWindow::ExitAction()"));

            // Terminate the application.  Note that just "closing" the window
            // will trigger the OnClosing event which cancels the application's
            // attempt to close.

            System.Windows.Application.Current.Shutdown();
        }

        private ICommand _stayAwakeToggleCommand;
        public ICommand StayAwakeToggleCommand => _stayAwakeToggleCommand ?? (_stayAwakeToggleCommand = new CommandHandler(() => StayAwakeToggleAction(), true));

        public void StayAwakeToggleAction()
        {
            _logger.Log(new LogEntry(LoggingEventType.Debug, "NoDoze::MainWindow::StayAwakeToggleAction()"));
            StayAwake = (!StayAwake);
        }

        public bool StayAwake
        {
            get => _stayAwake;
            set
            {
                if (value == StayAwake)
                {
                    _logger.Log(new LogEntry(LoggingEventType.Debug, "NoDoze::MainWindow::StayAwake::set() - Same state requested; no change actioned"));
                }
                else
                {
                    if (StayAwake)
                    {
                        _logger.Log(new LogEntry(LoggingEventType.Debug, "NoDoze::MainWindow::StayAwake::set() - Switching state to PermitSleeping"));
                        _sleepingService.PermitSleeping();
                    }
                    else
                    {
                        _logger.Log(new LogEntry(LoggingEventType.Debug, "NoDoze::MainWindow::StayAwake::set() - Switching state to PreventSleeping"));
                        _sleepingService.PreventSleeping();
                    }

                    // Indicate to the world (that cares) that this property has changed.
                    SetProperty(ref _stayAwake, value);

                    // A side effect will be that the status message will change...also inform the world.
                    OnPropertyChanged(StayAwakeStatusMessage);
                }
            }
        }

        public string StayAwakeStatusMessage => (StayAwake) ? StayAwakeSleepingPreventedStatus : StayAwakeSleepingPermittedStatus;
    }
}
