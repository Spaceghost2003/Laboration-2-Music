using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Laboration_2_Music;


    class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Action<object> execute, Func<object?, bool> canExecute = null)
        {
            ArgumentNullException.ThrowIfNull(execute);
            this._execute = execute;
            this._canExecute = canExecute;
        }

        public void RaiseCanExecuteChanged() => CanExecuteChanged.Invoke(this, EventArgs.Empty);
        public bool CanExecute(object? parameter)
        {
            return _canExecute is null ? true : _canExecute(parameter);
        }


        public void Execute(object? parameter)
        {
            _execute(parameter);
        }



    }
