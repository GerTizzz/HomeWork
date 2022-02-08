using HomeWork.Infrastructure;
using HomeWork.Infrastructure.Commands;
using HomeWork.Infrastructure.Services;
using HomeWork.Model;
using HomeWork.View;
using System;
using System.Collections.ObjectModel;
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

        //Свойство списка с книгами
        public ObservableCollection<Book> Books { get; } = new ObservableCollection<Book>();

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
            Books = new ObservableCollection<Book>(_dataBaseService.ReadFromDataBase());
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
                addBookViewModel.ShowAddButton = true;
                add.DataContext = addBookViewModel;
                add.ShowDialog();
                _dataBaseService.WriteToDataBase(SelectedBook);
                Books.Add(addBookViewModel.SelectedBook);
                SelectedBook = Books.LastOrDefault();
                SelectedBook.BookId = _dataBaseService.GetLastBookId();
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
                _dataBaseService.DeleteBookDataBase(SelectedBook.BookId);
                //Удаляю книгу из списка
                int index = Books.IndexOf(SelectedBook);
                Books.RemoveAt(index);
                //Делаю первую книгу из списка выбранной
                SelectedBook = index >= Books.Count ? Books[index - 1] : Books[index + 1];
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
                AddBookViewModel addBookViewModel = new AddBookViewModel();
                addBookViewModel.SelectedBook = SelectedBook;
                addBookViewModel.ShowEditButton = true;
                add.DataContext = addBookViewModel;
                add.ShowDialog();
                _dataBaseService.EditBookDataBase(SelectedBook);
            }
            catch (Exception exc)
            {
                _dialogService.ShowMessage(exc.Message);
            }
        }
    }
}