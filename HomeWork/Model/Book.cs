using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork.Model
{
    internal class Book
    {
        private string _Name;

        public string Name
        {
            get => _Name;
            set => _Name = value;
        }

        private string _Id;
        private string _Author;
        private string YearCreation;
        private string ISBN;
        private string Description;

    }
}