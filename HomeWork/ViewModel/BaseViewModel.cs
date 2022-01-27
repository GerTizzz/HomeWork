using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HomeWork.ViewModel
{
    class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChange([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        //метод позволяет избежать циклов обновления привязок
        //плюс через него мне интуитивно проще понимать какое поле привязывается, т.к. передаю не имя, а ссылку на объект
        public bool SetOnPropertyChanged<T>(ref T field, T value, [CallerMemberName] string PropertyName = null)
        {
            if (Equals(field, value))
            {
                return false;
            }
            field = value;
            OnPropertyChange(PropertyName);
            return true;
        }
    }
}