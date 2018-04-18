using Ninject;
using System.Reflection;
using System.Windows.Input;

using NoDoze.Bindings;
using NoDoze.Helpers;
using NoDoze.Interfaces;
using NoDoze.Logging;

namespace NoDoze.ViewModels
{
    public class MainWindow
    {
        private ILogger logger = DIFactory.Resolve<ILogger>();

        public MainWindow()
        {
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
    }
}
