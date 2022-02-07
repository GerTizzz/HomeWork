using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HomeWork.Model
{
    internal class Book : INotifyPropertyChanged
    {
        private string _name;
        private int _id;
        private string _author;
        private int _creationYear;
        private string _ISBN;
        private byte[] _cover;
        private string _description;

        public string BookName
        {
            get => _name;
            set => CanOnPropertyChanged(ref _name, value);
        }

        public int BookId
        {
            get => _id;
            set => CanOnPropertyChanged(ref _id, value);
        }

        public string BookAuthor
        {
            get => _author;
            set => CanOnPropertyChanged(ref _author, value);
        }

        public int BookDate
        {
            get => _creationYear;
            set => CanOnPropertyChanged(ref _creationYear, value);
        }

        public string BookISBN
        {
            get => _ISBN;
            set => CanOnPropertyChanged(ref _ISBN, value);
        }

        public byte[] BookCover
        {
            get => _cover;
            set => CanOnPropertyChanged(ref _cover, value);
        }

        public string BookDescription
        {
            get => _description;
            set => CanOnPropertyChanged(ref _description, value); 
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChange([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool CanOnPropertyChanged<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value))
            {
                return false;
            }
            field = value;
            OnPropertyChange(propertyName);
            return true;
        }
    }
}