using HomeWork.Infrastructure.Services;
using HomeWork.Model;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace HomeWork.Infrastructure
{
    internal class DataBaseService : IDataBaseService
    {
        private readonly string _connectionString;

        public DataBaseService()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DataBaseConnection"].ConnectionString;
        }

        public void DeleteBookDataBase(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "DELETE BooksTable WHERE BookId = (@id)";
                command.Parameters.AddWithValue("@id", id);
                command.Connection = connection;
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void EditBookDataBase(Book book)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "UPDATE BooksTable SET BookName = (@name), BookAuthor = (@author), BookISBN = (@isbn), " +
                    "BookCover = (@cover), BookDescription = (@descript), BookDate = (@date) WHERE BookId = (@id)";
                command.Parameters.AddWithValue("@name", book.BookName);
                command.Parameters.AddWithValue("@author", book.BookAuthor);
                command.Parameters.AddWithValue("@isbn", book.BookISBN);
                command.Parameters.AddWithValue("@cover", book.BookCover);
                command.Parameters.AddWithValue("@descript", book.BookDescription);
                command.Parameters.AddWithValue("@date", book.BookDate);
                command.Parameters.AddWithValue("@id", book.BookId);
                command.Connection = connection;
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public int GetLastBookId()
        {
            int resultId = 0;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "SELECT TOP 1 BookId FROM BooksTable ORDER BY BookId DESC";
                command.Connection = connection;
                connection.Open();
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    resultId = dataReader.GetInt32(0);
                }
                connection.Close();
            }
            return resultId;
        }

        public List<Book> ReadFromDataBase()
        {
            List<Book> Books = new List<Book>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "SELECT * FROM BooksTable";
                command.Connection = connection;
                connection.Open();
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    Book newBook = new Book();
                    newBook.BookId = dataReader.GetInt32(0);
                    newBook.BookName = dataReader.GetString(1);
                    newBook.BookAuthor = dataReader.GetString(2);                    
                    newBook.BookISBN = dataReader.GetString(3);
                    newBook.BookCover = (byte[])dataReader["BookCover"];
                    newBook.BookDescription = dataReader.GetString(5);
                    newBook.BookDate = dataReader.GetInt32(6);                    
                    Books.Add(newBook);
                }
                connection.Close();
            }
            return Books;
        }

        public void WriteToDataBase(Book book)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "INSERT BooksTable VALUES ((@name), (@author), (@isbn), (@cover), (@descript), (@date))";
                command.Parameters.AddWithValue("@name", book.BookName);
                command.Parameters.AddWithValue("@author", book.BookAuthor);
                command.Parameters.AddWithValue("@isbn", book.BookISBN);
                command.Parameters.AddWithValue("@cover", book.BookCover);
                command.Parameters.AddWithValue("@descript", book.BookDescription);
                command.Parameters.AddWithValue("@date", book.BookDate);
                command.Connection = connection;
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}