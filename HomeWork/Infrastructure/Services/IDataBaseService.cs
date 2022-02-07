using HomeWork.Model;
using System.Collections.Generic;

namespace HomeWork.Infrastructure.Services
{
    interface IDataBaseService
    {
        void WriteToDataBase(string connectionString, Book book);
        List<Book> ReadFromDataBase(string connectionString);
        void EditBookDataBase(string connectionString, Book book);
        void DeleteBookDataBase(string connectionString, int id);
        int GetLastBookId(string connectionString);
    }
}