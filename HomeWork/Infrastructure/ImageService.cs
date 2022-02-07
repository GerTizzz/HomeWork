using HomeWork.Infrastructure.Services;
using System.IO;

namespace HomeWork.Infrastructure
{
    class ImageService : IGetImageService
    {
        public byte[] OpenFile(string path)//Метод считывания файла изображения
        {
            byte[] file = new byte[0];
            using (FileStream fs = File.OpenRead(path))
            {
                file = new byte[fs.Length];
                fs.Read(file, 0, file.Length);
            }
            return file;
        }
    }
}