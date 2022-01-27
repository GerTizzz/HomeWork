using Microsoft.Win32;
using System.Windows;
using HomeWork.Infrastructure.Services;

namespace HomeWork.Infrastructure
{
    class WindowDialogService : IDialogService
    {
        public string FilePath { get; set; }

        public bool OpenFileDialog()
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

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}