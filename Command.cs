using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SocialApp
{
    public class Command : ICommand

    {
        private Action<object?> _action;
        private Func<object?, bool>? _canExecute;

        public Command(Action<object?> action, Func<object?, bool>? canExecute = null)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }
            else
            {
                return _canExecute(parameter);
            }
        }

        public void Execute(object? parameter)
        {
            if (_action != null)
            {
                _action(parameter);
            }
        }
        public void PosodobiCanExecute()
        {
            if(CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}
