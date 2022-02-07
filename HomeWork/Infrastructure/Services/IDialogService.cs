namespace HomeWork.Infrastructure.Services
{
    interface IDialogService//Сервис взаимодействия с пользователем при помощи дополнительных окон
    {
        string FilePath { get; set; }//путь до выбранного файла

        void ShowMessage(string message);//окно с сообщением для пользователя

        bool OpenFileDialog();//окно для выбором файла

    }
}