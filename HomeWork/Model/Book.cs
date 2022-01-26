using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HomeWork.Model
{
    internal class Book : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public int Id { get; set; }

        private string _Author;

        public string Author
        {
            get => _Author;
            set => SetOnPropertyChanged(ref _Author, value);
        }

        public DateTime YearCreation;

        //public Image Cover { get; set; }
        private string _ISBN;

        public string ISBN
        {
            get => _ISBN;
            set => SetOnPropertyChanged(ref _ISBN, value);
        }

        public string Description { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChange([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

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