using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace weatherAssistant.Commands
{
    public class BaseCommand : ICommand
    {
        #region Fields

        readonly Action _execute;
        readonly Action<object> _executeWithParameter;
        readonly Predicate<object> _canExecute;

        #endregion // Fields

        #region Constructors

        /// <summary>
        /// Creates a new command that can always execute.
        /// </summary>
        /// <param name="executeWithParameter">The execution logic.</param>
        public BaseCommand(Action<object> executeWithParameter)
            : this(executeWithParameter, null)
        {
        }

        public BaseCommand(Action execute)
        {
            _execute = execute;
            _canExecute = null;
        }

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="executeWithParameter">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public BaseCommand(Action<object> executeWithParameter, Predicate<object> canExecute)
        {
            if (executeWithParameter == null)
                throw new ArgumentNullException("executeWithParameter");

            _executeWithParameter = executeWithParameter;
            _canExecute = canExecute;
        }

        #endregion // Constructors

        #region ICommand Members

        [DebuggerStepThrough]
        public bool CanExecute(object parameters)
        {
            return _canExecute == null ? true : _canExecute(parameters);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameters)
        {
            if (parameters != null)
                _executeWithParameter(parameters);
            else
                _execute();
        }

        #endregion // ICommand Members
    }
}
