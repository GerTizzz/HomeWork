using HomeWork.Model;
using System.Collections.Generic;

namespace HomeWork.Infrastructure.Services
{
    interface IDataBaseService
    {
        void WriteToDataBase(Book book);
        List<Book> ReadFromDataBase();
        void EditBookDataBase(Book book);
        void DeleteBookDataBase(int id);
        int GetLastBookId();
    }
}