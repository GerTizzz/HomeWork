namespace HomeWork.Infrastructure.Services
{
    interface IGetImageService//сервис считывания файла
    {
        byte[] OpenFile(string path);//метод считывания файла изображения
    }
}