using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PetrolStationMVVM.ViewModel
{
    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private Action mAction;

        public RelayCommand(Action mAction)
        {
          
            this.mAction = mAction;

        }


        public bool CanExecute(object parameter)
        {

            return true;
        }

        public void Execute(object parameter)
        {
            mAction();
        }
    }
}
