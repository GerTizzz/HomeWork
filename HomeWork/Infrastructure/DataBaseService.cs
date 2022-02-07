﻿using HomeWork.Infrastructure.Services;
using HomeWork.Model;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace HomeWork.Infrastructure
{
    internal class DataBaseService : IDataBaseService
    {
        public void DeleteBookDataBase(string connectionString, int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
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

        public void EditBookDataBase(string connectionString, Book book)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "UPDATE BooksTable SET BookName = (@name), BookAuthor = (@author), BookDate = (@date), BookISBN = (@isbn), " +
                    "BookCover = (@cover), BookDescription = (@descript) WHERE BookId = (@id)";
                command.Parameters.AddWithValue("@name", book.BookName);
                command.Parameters.AddWithValue("@author", book.BookAuthor);
                command.Parameters.AddWithValue("@date", book.BookDate.ToString());
                command.Parameters.AddWithValue("@isbn", book.BookISBN);
                command.Parameters.AddWithValue("@cover", book.BookCover);
                command.Parameters.AddWithValue("@descript", book.BookDescription);
                command.Parameters.AddWithValue("@id", book.BookId);
                command.Connection = connection;
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public int GetLastBookId(string connectionString)
        {
            int resultId = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
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

        public List<Book> ReadFromDataBase(string connectionString)
        {
            List<Book> Books = new List<Book>();
            using (SqlConnection connection = new SqlConnection(connectionString))
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
                    newBook.BookDate = dataReader.GetDateTime(3).Year;
                    newBook.BookISBN = dataReader.GetString(4);
                    newBook.BookDescription = dataReader.GetString(6);
                    newBook.BookCover = (byte[])dataReader["BookCover"];
                    Books.Add(newBook);
                }
                connection.Close();
            }
            return Books;
        }

        public void WriteToDataBase(string connectionString, Book book)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "INSERT BooksTable VALUES ((@name), (@author), (@date), (@isbn), (@cover), (@descript))";
                command.Parameters.AddWithValue("@name", book.BookName);
                command.Parameters.AddWithValue("@author", book.BookAuthor);
                command.Parameters.AddWithValue("@date", book.BookDate.ToString());
                command.Parameters.AddWithValue("@isbn", book.BookISBN);
                command.Parameters.AddWithValue("@cover", book.BookCover);
                command.Parameters.AddWithValue("@descript", book.BookDescription);
                command.Connection = connection;
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}