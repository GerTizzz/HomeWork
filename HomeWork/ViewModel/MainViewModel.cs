using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Configuration;
using HomeWork.Model;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HomeWork.ViewModel
{
    class MainViewModel : BaseViewModel
    {
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
        public MainViewModel()
        {
            _ConnectionString = ConfigurationManager.ConnectionStrings["DataBaseConnection"].ConnectionString;
            GetBookShortList();
        }
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
            }
            catch (Exception exc)
            {
                //Прописать диалоговое окно
            }
        }

        private async void GetFullBookInformation(int id)
        {
            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "SELECT * FROM BooksTable WHERE BookId = " + id;
                command.Connection = connection;
                await connection.OpenAsync();
                SqlDataReader dataReader = await command.ExecuteReaderAsync();
                while (await dataReader.ReadAsync())
                {
                    Book newBook = new Book();
                    _SelectedBook.YearCreation = dataReader.GetDateTime(3);
                    _SelectedBook.ISBN = dataReader.GetString(4);
                    //newBook. = dataReader.GetBytes(5);
                    _SelectedBook.Description = dataReader.GetString(6);
                    Books.Add(newBook);
                }
            }
        }

        private void AddNewBook()
        {

        }

        private void DeleteSelectedBook()
        {

        }
    }
}