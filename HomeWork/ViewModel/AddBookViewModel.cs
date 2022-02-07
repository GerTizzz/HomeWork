using HomeWork.Infrastructure;
using HomeWork.Infrastructure.Commands;
using HomeWork.Infrastructure.Services;
using HomeWork.Model;
using System;
using System.Configuration;
using System.Windows.Input;

namespace HomeWork.ViewModel
{
    internal class AddBookViewModel : BaseViewModel
    {
        // Сервис, вызывающий окно выбора файла и окно сообщения
        private readonly IDialogService _dialogService;
        // Сервис, считывающий выбранный файл
        private readonly IGetImageService _imageService;
        private readonly IDataBaseService _dataBaseService;
        private readonly string _connectionString;
        private Book _selectedBook;
        public ICommand AddBookCommand { get; }
        public ICommand EditBookCommand { get; }
        public ICommand SetNewCoverCommand { get; }

        public Book SelectedBook
        {
            get => _selectedBook;
            set => CanOnPropertyChanged(ref _selectedBook, value);
        }

        public AddBookViewModel()
        {
            AddBookCommand = new MainCommand(OnAddBookCommandExecuted, CanAddBookCommandExecute);
            EditBookCommand = new MainCommand(OnEditBookCommandExecuted, CanEditBookCommandExecute);
            SetNewCoverCommand = new MainCommand(OnSetNewCoverCommandExecuted, CanSetNewCoverCommandExecute);
            _dialogService = new WindowDialogService();
            _imageService = new ImageService();
            _dataBaseService = new DataBaseService();
            _connectionString = ConfigurationManager.ConnectionStrings["DataBaseConnection"].ConnectionString;
        }
        private bool CanAddBookCommandExecute(object p) => true;

        private void OnAddBookCommandExecuted(object p)
        {
            try
            {
                _dataBaseService.WriteToDataBase(_connectionString, SelectedBook);                
            }
            catch (Exception exc)
            {
                _dialogService.ShowMessage(exc.Message);
            }
        }


        private bool CanEditBookCommandExecute(object p) => true;

        private void OnEditBookCommandExecuted(object p)
        {
            try
            {
                _dataBaseService.EditBookDataBase(_connectionString, SelectedBook);
            }
            catch (Exception exc)
            {
                _dialogService.ShowMessage(exc.Message);
            }
        }


        private bool CanSetNewCoverCommandExecute(object p) => true;

        private void OnSetNewCoverCommandExecuted(object p)
        {
            try
            {
                if (_dialogService.OpenFileDialog() == true)
                {
                    SelectedBook.BookCover = _imageService.OpenFile(_dialogService.FilePath);
                }
            }
            catch (Exception exc)
            {
                _dialogService.ShowMessage(exc.Message);
            }
        }
    }
}