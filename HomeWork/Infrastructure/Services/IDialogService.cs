namespace HomeWork.Infrastructure.Services
{
    interface IDialogService
    {
        void ShowMessage(string message);

        string FilePath { get; set; }

        bool OpenFileDialog();
    }
}