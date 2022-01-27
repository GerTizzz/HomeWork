using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HomeWork.Model
{
    internal class Book : INotifyPropertyChanged
    {
        private string _Name;

        public string BookName
        {
            get => _Name;
            set => SetOnPropertyChanged(ref _Name, value);
        }

        private int _Id;

        public int BookId
        {
            get => _Id;
            set => SetOnPropertyChanged(ref _Id, value);
        }

        private string _Author;

        public string BookAuthor
        {
            get => _Author;
            set => SetOnPropertyChanged(ref _Author, value);
        }

        private DateTime _YearCreation;

        public DateTime BookDate
        {
            get => _YearCreation;
            set => SetOnPropertyChanged(ref _YearCreation, value);
        }

        private string _ISBN;

        public string BookISBN
        {
            get => _ISBN;
            set => SetOnPropertyChanged(ref _ISBN, value);
        }

        private byte[] _Cover;

        public byte[] BookCover
        {
            get => _Cover;
            set => SetOnPropertyChanged(ref _Cover, value);
        }

        private string _Description;

        public string BookDescription
        {
            get => _Description;
            set => SetOnPropertyChanged(ref _Description, value); 
        }

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