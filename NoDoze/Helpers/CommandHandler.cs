using System;
using System.Windows.Input;

namespace NoDoze.Helpers
{
    public class CommandHandler : ICommand
    {
        private Action action;
        private bool canExecute;

        public CommandHandler(Action _action, bool _canExecute)
        {
            action = _action;
            canExecute = _canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            action();
        }
    }
}
