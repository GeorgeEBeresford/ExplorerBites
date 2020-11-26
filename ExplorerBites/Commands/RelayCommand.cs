using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExplorerBites.Commands
{
    public class RelayCommand : ICommand
    {
        public RelayCommand (Action executedAction)
        {
            ExecutedAction = executedAction;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            ExecutedAction.Invoke();
        }

        private Action ExecutedAction { get; }

        public event EventHandler CanExecuteChanged;
    }
}
