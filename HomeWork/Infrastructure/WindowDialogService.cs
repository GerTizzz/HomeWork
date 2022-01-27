using Microsoft.Win32;
using System.Windows;
using HomeWork.Infrastructure.Services;

namespace HomeWork.Infrastructure
{
    class WindowDialogService : IDialogService
    {
        public string FilePath { get; set; }//Путь до выбранного файла изображения

        public bool OpenFileDialog()//Метод открытия кона для выбора файла
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "jpg files (*.jpg)|*.jpg";
            if (ofd.ShowDialog() == true)
            {
                FilePath = ofd.FileName;
                return true;
            }
            return false;
        }

        public void ShowMessage(string message)//Метод вызова окна сообщений
        {
            MessageBox.Show(message);
        }
    }
}