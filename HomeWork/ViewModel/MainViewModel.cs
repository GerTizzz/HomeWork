using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Configuration;
using HomeWork.Model;
using HomeWork.Infrastructure.Commands;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HomeWork.ViewModel
{
    class MainViewModel : BaseViewModel
    {

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
            AddBook = new MainCommand(OnAddBookCommandExecuted, CanAddBookCommandExecute);
            GetBook = new MainCommand(OnGetBookCommandExecuted, CanGetBookCommandExecute);
            _ConnectionString = ConfigurationManager.ConnectionStrings["DataBaseConnection"].ConnectionString;
            GetBookShortList();
        }

        #endregion

        #region Команды

        public ICommand AddBook { get; }

        private bool CanAddBookCommandExecute(object p) => true;

        private void OnAddBookCommandExecuted(object p)
        {

        }

        public ICommand GetBook { get; }

        private bool CanGetBookCommandExecute(object p) => true;

        private async void OnGetBookCommandExecuted(object p)
        {
            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "SELECT * FROM BooksTable WHERE BookId = " + _SelectedBook.Id;
                command.Connection = connection;
                await connection.OpenAsync();
                SqlDataReader dataReader = await command.ExecuteReaderAsync();
                while (await dataReader.ReadAsync())
                {
                    _SelectedBook.YearCreation = dataReader.GetDateTime(3);
                    _SelectedBook.ISBN = dataReader.GetString(4);
                    _SelectedBook.Description = dataReader.GetString(6);
                }
            }
        }

        #endregion

        private async void GetBookShortList()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_ConnectionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.CommandText = "SELECT BookId, BookName, BookAuthor FROM BooksTable";
                    command.Connection = connection;
                    await connection.OpenAsync();
                    SqlDataReader dataReader = await command.ExecuteReaderAsync();
                    while (await dataReader.ReadAsync())
                    {
                        Book newBook = new Book();
                        newBook.Id = dataReader.GetInt32(0);
                        newBook.Name = dataReader.GetString(1);
                        newBook.Author = dataReader.GetString(2);
                        Books.Add(newBook);
                    }
                }
                _SelectedBook = _Books.FirstOrDefault();
            }
            catch (Exception exc)
            {
                //Прописать диалоговое окно
            }
        }

        private void GetFullBookInformation(int id)
        {

        }

        private void AddNewBook()
        {

        }

        private void DeleteSelectedBook()
        {

        }
    }
}