using System;
using System.Data.SqlClient;
using System.Linq;
using System.Configuration;
using System.Collections.ObjectModel;
using System.Windows.Input;

using HomeWork.Model;
using HomeWork.Infrastructure;
using HomeWork.Infrastructure.Commands;
using HomeWork.Infrastructure.Services;

namespace HomeWork.ViewModel
{
    class MainViewModel : BaseViewModel
    {

        #region Сервисы работы с пользователем и файлами

        private IDialogService _DialogService; // Сервис, вызывающий окно выбора файла и окно сообщения
        private IGetImageService _ImageService; // Сервис, считывающий выбранный файл

        #endregion 

        #region Поля и Свойства

        private readonly string _ConnectionString;

        private ObservableCollection<Book> _Books = new ObservableCollection<Book>();

        public ObservableCollection<Book> Books
        {
            get => _Books;
        }

        private Book _SelectedBook;

        public Book SelectedBook
        {
            get => _SelectedBook;
            set => SetOnPropertyChanged(ref _SelectedBook, value);
        }

        #endregion

        #region Конструктор

        public MainViewModel()
        {
            AddBookCommand = new MainCommand(OnAddBookCommandExecuted, CanAddBookCommandExecute);
            GetBookCommand = new MainCommand(OnGetBookCommandExecuted, CanGetBookCommandExecute);
            GetShortListCommand = new MainCommand(OnGetShortListCommandExecuted, CanGetShortListCommandExecute);
            GetNewCoverCommand = new MainCommand(OnGetNewCoverCommandExecuted, CanGetNewCoverCommandExecute);
            SetUpdatedDataCommand = new MainCommand(OnSetUpdatedDataCommandExecuted, CanSetUpdatedDataCommandExecute);
            _DialogService = new WindowDialogService();
            _ImageService = new ImageService();
            _ConnectionString = ConfigurationManager.ConnectionStrings["DataBaseConnection"].ConnectionString;
        }

        #endregion

        #region Команды

        #region Команда добавления книги

        public ICommand AddBookCommand { get; }

        private bool CanAddBookCommandExecute(object p) => true;

        private void OnAddBookCommandExecuted(object p)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_ConnectionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.CommandText = "INSERT BooksTable VALUES ((@name), (@author), (@date), (@isbn), (@cover), (@descript))";
                    command.Parameters.AddWithValue("@name", SelectedBook.BookName);
                    command.Parameters.AddWithValue("@author", SelectedBook.BookAuthor);
                    command.Parameters.AddWithValue("@date", SelectedBook.BookDate.ToString("yyyy"));
                    command.Parameters.AddWithValue("@isbn", SelectedBook.BookISBN);
                    command.Parameters.AddWithValue("@cover", SelectedBook.BookCover);
                    command.Parameters.AddWithValue("@descript", SelectedBook.BookDescription);
                    command.Connection = connection;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                Books.Add(SelectedBook);
            }
            catch (Exception exc)
            {
                _DialogService.ShowMessage(exc.Message);
            }
        }

        #endregion

        #region Команда удаления книги

        public ICommand RemoveBookCommand { get; }

        private bool CanRemoveBookCommandExecute(object p) => true;

        private void OnRemoveBookCommandExecuted(object p)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_ConnectionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.CommandText = "DELETE BooksTable WHERE BookId = (@id)";
                    command.Parameters.AddWithValue("@id", SelectedBook.BookId);
                    command.Connection = connection;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                Books.Remove(SelectedBook);
                SelectedBook = Books.FirstOrDefault();
            }
            catch (Exception exc)
            {
                _DialogService.ShowMessage(exc.Message);
            }
        }

        #endregion

        #region Команда получения книг

        public ICommand GetShortListCommand { get; }

        private bool CanGetShortListCommandExecute(object p) => true;

        private void OnGetShortListCommandExecuted(object p)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_ConnectionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.CommandText = "SELECT * FROM BooksTable";
                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        Book newBook = new Book();
                        newBook.BookId = dataReader.GetInt32(0);
                        newBook.BookName = dataReader.GetString(1);
                        newBook.BookAuthor = dataReader.GetString(2);
                        newBook.BookDate = dataReader.GetDateTime(3);
                        newBook.BookISBN = dataReader.GetString(4);
                        newBook.BookDescription = dataReader.GetString(6);
                        newBook.BookCover = (byte[])dataReader["BookCover"];
                        Books.Add(newBook);
                    }
                    connection.Close();
                }
                SelectedBook = Books.FirstOrDefault();
            }
            catch (Exception exc)
            {
                _DialogService.ShowMessage(exc.Message);
            }
        }

        #endregion

        #region Команда получения выбранной книги

        public ICommand GetBookCommand { get; }

        private bool CanGetBookCommandExecute(object p) => true;

        private void OnGetBookCommandExecuted(object p)
        {
            try
            {
                SelectedBook = Books.FirstOrDefault(x => x.BookId == (int)p);
            }
            catch (Exception exc)
            {
                _DialogService.ShowMessage(exc.Message);
            }
        }

        #endregion

        #region Команда замены изображения обложки книги

        public ICommand GetNewCoverCommand { get; }

        private bool CanGetNewCoverCommandExecute(object p) => true;

        private void OnGetNewCoverCommandExecuted(object p)
        {
            try
            {
                if (_DialogService.OpenFileDialog() == true)
                {
                    SelectedBook.BookCover = _ImageService.OpenFile(_DialogService.FilePath);
                }
            }
            catch (Exception exc)
            {
                _DialogService.ShowMessage(exc.Message);
            }
        }

        #endregion

        #region Команда обновления данных о книге

        public ICommand SetUpdatedDataCommand { get; }

        private bool CanSetUpdatedDataCommandExecute(object p) => true;

        private void OnSetUpdatedDataCommandExecuted(object p)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_ConnectionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.CommandText = "UPDATE BooksTable SET BookName = (@name), BookAuthor = (@author), BookDate = (@date), BookISBN = (@isbn), " +
                        "BookCover = (@cover), BookDescription = (@descript) WHERE BookId = (@id)";
                    command.Parameters.AddWithValue("@name", SelectedBook.BookName);
                    command.Parameters.AddWithValue("@author", SelectedBook.BookAuthor);
                    command.Parameters.AddWithValue("@date", SelectedBook.BookDate.ToString("yyyy"));
                    command.Parameters.AddWithValue("@isbn", SelectedBook.BookISBN);
                    command.Parameters.AddWithValue("@cover", SelectedBook.BookCover);
                    command.Parameters.AddWithValue("@descript", SelectedBook.BookDescription);
                    command.Parameters.AddWithValue("@id", SelectedBook.BookId);
                    command.Connection = connection;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception exc)
            {
                _DialogService.ShowMessage(exc.Message);
            }
        }

        #endregion

        #endregion

    }
}