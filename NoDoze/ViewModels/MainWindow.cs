using Ninject;
using System.Reflection;
using System.Windows.Input;

using NoDoze.Bindings;
using NoDoze.Helpers;
using NoDoze.Interfaces;
using NoDoze.Logging;

namespace NoDoze.ViewModels
{
    public class MainWindow : ViewModelBase
    {
        private ISleepingService sleepingService = DIFactory.Resolve<ISleepingService>();
        private ILogger logger = DIFactory.Resolve<ILogger>();

        private string StayAwake_SleepingPermittedStatus = "NoDoze Inactive - Sleep Enabled";
        private string StayAwake_SleepingPreventedStatus = "NoDoze Active - Sleep Disabled";
        private bool stayAwake;

        public MainWindow()
        {
            // The default here is to prevent sleeping as it requires explicit
            // actions by the user to permit sleeping....after all, it's the 
            // point of the application.

            StayAwake = true;
        }

        private ICommand exitCommand;
        public ICommand ExitCommand
        {
            get { return exitCommand ?? (exitCommand = new CommandHandler(() => ExitAction(), true)); }
        }

        public void ExitAction()
        {
            logger.Log(new LogEntry(LoggingEventType.Debug, "NoDoze::MainWindow::ExitAction()"));

            // Terminate the application.  Note that just "closing" the window
            // will trigger the OnClosing event which cancels the application's
            // attempt to close.

            System.Windows.Application.Current.Shutdown();
        }

        private ICommand stayAwakeToggleCommand;
        public ICommand StayAwakeToggleCommand
        {
            get { return stayAwakeToggleCommand ?? (stayAwakeToggleCommand = new CommandHandler(() => StayAwakeToggleAction(), true)); }
        }

        public void StayAwakeToggleAction()
        {
            logger.Log(new LogEntry(LoggingEventType.Debug, "NoDoze::MainWindow::StayAwakeToggleAction()"));
            StayAwake = (!StayAwake);
        }

        public bool StayAwake
        {
            get
            {
                return stayAwake;
            }

            set
            {
                if (value == StayAwake)
                {
                    logger.Log(new LogEntry(LoggingEventType.Debug, "NoDoze::MainWindow::StayAwake::set() - Same state requested; no change actioned"));
                }
                else
                {
                    if (StayAwake)
                    {
                        logger.Log(new LogEntry(LoggingEventType.Debug, "NoDoze::MainWindow::StayAwake::set() - Switching state to PermitSleeping"));
                        sleepingService.PermitSleeping();
                    }
                    else
                    {
                        logger.Log(new LogEntry(LoggingEventType.Debug, "NoDoze::MainWindow::StayAwake::set() - Switching state to PreventSleeping"));
                        sleepingService.PreventSleeping();
                    }

                    // Indicate to the world (that cares) that this property has changed.
                    SetProperty(ref stayAwake, value);

                    // A side effect will be that the status message will change...also inform the world.
                    OnPropertyChanged(StayAwakeStatusMessage);
                }
            }
        }

        public string StayAwakeStatusMessage
        { 
            get { return StayAwake ? StayAwake_SleepingPreventedStatus : StayAwake_SleepingPermittedStatus; }
        }
    }
}
