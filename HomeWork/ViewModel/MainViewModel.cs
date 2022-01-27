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

        //Строка подключения к БД
        private readonly string _ConnectionString;

        //Список, который будет отображать книги
        private ObservableCollection<Book> _Books = new ObservableCollection<Book>();

        //Свойство списка с книгами
        public ObservableCollection<Book> Books
        {
            get => _Books;
        }

        //Выбранная книга
        private Book _SelectedBook;

        //Свойство выбранной книги
        public Book SelectedBook
        {
            get => _SelectedBook;
            set => SetOnPropertyChanged(ref _SelectedBook, value);
        }

        #endregion

        #region Конструктор

        public MainViewModel()
        {
            //Создаю новые команды
            AddBookCommand = new MainCommand(OnAddBookCommandExecuted, CanAddBookCommandExecute);
            GetBookCommand = new MainCommand(OnGetBookCommandExecuted, CanGetBookCommandExecute);
            GetShortListCommand = new MainCommand(OnGetShortListCommandExecuted, CanGetShortListCommandExecute);
            GetNewCoverCommand = new MainCommand(OnGetNewCoverCommandExecuted, CanGetNewCoverCommandExecute);
            SetUpdatedDataCommand = new MainCommand(OnSetUpdatedDataCommandExecuted, CanSetUpdatedDataCommandExecute);
            RemoveBookCommand = new MainCommand(OnRemoveBookCommandExecuted, CanRemoveBookCommandExecute);
            //Инициализирую сервисы
            _DialogService = new WindowDialogService();
            _ImageService = new ImageService();
            //Получаю строку подключения
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
                if (SelectedBook == null)//Проверяю на пустоту
                    SelectedBook = new Book();
                //Добавляю данные по книги
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
                //Получаю сгенерированный для этой книги Id
                using (SqlConnection connection = new SqlConnection(_ConnectionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.CommandText = "SELECT TOP 1 BookId FROM BooksTable ORDER BY BookId DESC";
                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        SelectedBook.BookId = dataReader.GetInt32(0);
                    }
                    connection.Close();
                }
                //Добавляю книгу в список
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
                if (Books.Count < 1)//Проверяю список на наличие книг
                    return;
                //Удаляю по Id книгу
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
                //Удаляю книгу из списка
                Books.Remove(Books.SingleOrDefault(x => x.BookId == SelectedBook.BookId));
                //Делаю первую книгу из списка выбранной
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
                //Получаю все книги из БД
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
                        //Заношу в список книгу из БД
                        Books.Add(newBook);
                    }
                    connection.Close();
                }
                //Делаю первую книгу выбранной
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
                if (p == null)//Проверяю был ли передан Id
                    SelectedBook = Books.FirstOrDefault();//Выбираю просто первую книгу из списка
                else//Выбираю книгу из списка по Id
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
                    if (SelectedBook == null)//Проверяю существует ли выбранная книга
                        SelectedBook = new Book();
                    //Присваиваю обложке выбранной книги изображение
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
                if (Books.Count < 1)//Проверяю существуют ли книги в списке
                    return;
                //Обновляю все (для простоты) поля
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