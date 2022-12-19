using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.ViewModels.Base
{
    internal abstract class ViewModel:INotifyPropertyChanged//Базовый класс модели предстваления
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName]string PropertyName=null)//Уведомляет вью о изменении свойства
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        
        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName]string PropertyName = null)//регестрирует свойство уведомляет вью
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(PropertyName);
            return true;
        }


        
    }
}
