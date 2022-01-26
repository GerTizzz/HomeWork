using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;

namespace HomeWork.ViewModel
{
    class MainViewModel : BaseViewModel
    {
        private readonly string _ConnectionString;
        public MainViewModel()
        {
            _ConnectionString = ConfigurationManager.ConnectionStrings["DataBaseConnection"].ConnectionString;
            GetBookShortList();
        }

        private int _SelectedBookindex;

        public int SelectedBookIndex
        {
            get => _SelectedBookindex;
            set => SetOnPropertyChanged(ref _SelectedBookindex, value);
        }

        private List<string> _BooksShortList = new List<string>();

        public List<string> BooksShortList
        {
            get => _BooksShortList;
        }

        private string _Id;

        public string Id 
        {
            get => _Id;
            set => SetOnPropertyChanged(ref _Id, value);
        }

        private async void GetBookShortList()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_ConnectionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.CommandText = "SELECT BookName FROM BooksTable";
                    command.Connection = connection;
                    await connection.OpenAsync();
                    SqlDataReader dataReader = await command.ExecuteReaderAsync();
                    while (await dataReader.ReadAsync())
                    {
                        _BooksShortList.Add(dataReader.GetValue(0).ToString());
                    }
                }
            }
            catch (Exception exc)
            {
                //Прописать диалоговое окно
            }
        }

        private void AddNewBook()
        {

        }

        private void DeleteSelectedBook()
        {

        }
    }
}