using HomeWork.Infrastructure;
using HomeWork.Infrastructure.Commands;
using HomeWork.Infrastructure.Services;
using HomeWork.Model;
using HomeWork.View;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Windows.Input;

namespace HomeWork.ViewModel
{
    class MainViewModel : BaseViewModel
    {
        // Сервис, вызывающий окно выбора файла и окно сообщения
        private readonly IDialogService _dialogService;

        private readonly IDataBaseService _dataBaseService;

        //Выбранная книга
        private Book _selectedBook;
        //Строка подключения к БД
        private readonly string _connectionString;

        //Свойство списка с книгами
        public ObservableCollection<Book> Books { get; set; } = new ObservableCollection<Book>();

        //Свойство выбранной книги
        public Book SelectedBook
        {
            get => _selectedBook;
            set => CanOnPropertyChanged(ref _selectedBook, value);
        }
        public ICommand AddBookCommand { get; }
        public ICommand RemoveBookCommand { get; }
        public ICommand SetUpdatedDataCommand { get; }


        public MainViewModel()
        {
            //Создаю новые команды
            AddBookCommand = new MainCommand(OnAddBookCommandExecuted, CanAddBookCommandExecute);
            SetUpdatedDataCommand = new MainCommand(OnSetUpdatedDataCommandExecuted, CanSetUpdatedDataCommandExecute);
            RemoveBookCommand = new MainCommand(OnRemoveBookCommandExecuted, CanRemoveBookCommandExecute);
            //Инициализирую сервисы
            _dialogService = new WindowDialogService();
            _dataBaseService = new DataBaseService();
            //Получаю строку подключения
            _connectionString = ConfigurationManager.ConnectionStrings["DataBaseConnection"].ConnectionString;
            Books = new ObservableCollection<Book>(_dataBaseService.ReadFromDataBase(_connectionString));
            SelectedBook = Books.FirstOrDefault();
        }
        

        private bool CanAddBookCommandExecute(object p) => true;

        private void OnAddBookCommandExecuted(object p)
        {
            try
            {
                AddBookWindow add = new AddBookWindow();
                AddBookViewModel addBookViewModel = new AddBookViewModel();
                addBookViewModel.SelectedBook = new Book();
                add.Title = "Добавить книгу";
                add.DataContext = addBookViewModel;
                add.ShowDialog();
                Books.Add(addBookViewModel.SelectedBook);
                SelectedBook = Books.LastOrDefault();
                SelectedBook.BookId = _dataBaseService.GetLastBookId(_connectionString);
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessage(ex.Message);
            }
        }


        private bool CanRemoveBookCommandExecute(object p) => true;

        private void OnRemoveBookCommandExecuted(object p)
        {
            try
            {
                if (Books.Count < 1)//Проверяю список на наличие книг
                {
                    return;
                }
                _dataBaseService.DeleteBookDataBase(_connectionString, SelectedBook.BookId);
                //Удаляю книгу из списка
                Books.Remove(Books.SingleOrDefault(x => x.BookId == SelectedBook.BookId));
                //Делаю первую книгу из списка выбранной
                SelectedBook = Books.FirstOrDefault();
            }
            catch (Exception exc)
            {
                _dialogService.ShowMessage(exc.Message);
            }
        }          


        private bool CanSetUpdatedDataCommandExecute(object p) => true;

        private void OnSetUpdatedDataCommandExecuted(object p)
        {
            try
            {
                if (Books.Count < 1)
                {
                    return;
                }
                AddBookWindow add = new AddBookWindow();
                add.Title = "Редактировать книгу";
                AddBookViewModel viewModel = new AddBookViewModel();
                viewModel.SelectedBook = SelectedBook;
                add.DataContext = viewModel;
                add.ShowDialog();
            }
            catch (Exception exc)
            {
                _dialogService.ShowMessage(exc.Message);
            }
        }
    }
}