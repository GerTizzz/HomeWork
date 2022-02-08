using HomeWork.Infrastructure;
using HomeWork.Infrastructure.Commands;
using HomeWork.Infrastructure.Services;
using HomeWork.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Input;

namespace HomeWork.ViewModel
{
    internal class AddBookViewModel : BaseViewModel
    {
        // Сервис, вызывающий окно выбора файла и окно сообщения
        private readonly IDialogService _dialogService;
        // Сервис, считывающий выбранный файл
        private readonly IGetImageService _imageService;
        private Book _selectedBook;
        public ICommand AddBookCommand { get; }
        public ICommand EditBookCommand { get; }
        public ICommand SetNewCoverCommand { get; }

        public string ShowAddButton { get; set; } = "Hidden";
        public string ShowEditButton { get; set; } = "Hidden";

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
        }
        private bool CanAddBookCommandExecute(object p) => true;

        private void OnAddBookCommandExecuted(object p)
        {
            try
            {
                ValidateBook();                      
                if (p is Window window)
                {
                    CloseWindow(window);
                }
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
                ValidateBook();                
                if (p is Window window)
                {
                    CloseWindow(window);
                }
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

        //Хэлпер закрытия окна после внесения изменений/добавления
        private void CloseWindow(Window window)
        {
            window.Close();
        }

        //Хэлпер проверки внесенных данных
        private void ValidateBook()
        {
            List<ValidationResult> validationResultList = new List<ValidationResult>();
            ValidationContext context = new ValidationContext(SelectedBook);
            if (Validator.TryValidateObject(SelectedBook, context, validationResultList, true) is false)
            {
                string userMessage = string.Empty;
                foreach (var error in validationResultList)
                {
                    userMessage += error.ErrorMessage + "\n";
                }
                throw new Exception(userMessage);
            }
        }
    }
}