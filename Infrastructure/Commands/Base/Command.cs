using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp1.Infrastructure.Commands.Base;

namespace WpfApp1.Infrastructure.Commands.Base
{
    internal abstract class Command:ICommand//creating abstract class of commands
    {
        public event EventHandler CanExecuteChanged//system that notificate view
        {
            add =>CommandManager.RequerySuggested+=value; 
            remove =>CommandManager.RequerySuggested -= value;
        }

        public abstract bool CanExecute(object parameter);

        public abstract void Execute(object parameter);

       
    }
}
