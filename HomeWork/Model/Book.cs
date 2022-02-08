using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;

namespace HomeWork.Model
{
    internal class Book : INotifyPropertyChanged
    {
        private string _name;
        private int _id;
        private string _author;
        private int? _creationYear;
        private string _ISBN;
        private byte[] _cover;
        private string _description;

        [Required(ErrorMessage = "Не заполнено поле названия книги!")]
        [StringLength(100, MinimumLength = 1)]
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

        [Required(ErrorMessage = "Не заполнено поле автора книги!")]
        [StringLength(100, MinimumLength = 1)]
        public string BookAuthor
        {
            get => _author;
            set => CanOnPropertyChanged(ref _author, value);
        }
        [Required(ErrorMessage = "Не заполнено поле дата выхода книги!")]
        public int? BookDate
        {
            get => _creationYear;
            set => CanOnPropertyChanged(ref _creationYear, value);
        }

        [Required(ErrorMessage = "Не заполнено поле ISBN книги!")]
        [StringLength(30, MinimumLength = 20)]
        public string BookISBN
        {
            get => _ISBN;
            set => CanOnPropertyChanged(ref _ISBN, value);
        }

        [Required(ErrorMessage = "Не выбрана обложка книги!")]
        public byte[] BookCover
        {
            get => _cover;
            set => CanOnPropertyChanged(ref _cover, value);
        }

        [Required(ErrorMessage = "Не заполнено поле описание книги!")]
        [StringLength(1000, MinimumLength = 50)]
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