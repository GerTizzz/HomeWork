namespace HomeWork.Infrastructure.Services
{
    interface IDialogService//Сервис взаимодействия с пользователем при помощи дополнительных окон
    {
        void ShowMessage(string message);//окно с сообщением для пользователя

        string FilePath { get; set; }//путь до выбранного файла

        bool OpenFileDialog();//окно для выбором файла
    }
}