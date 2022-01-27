namespace HomeWork.Infrastructure.Services
{
    interface IGetImageService
    {
        byte[] OpenFile(string path);
    }
}