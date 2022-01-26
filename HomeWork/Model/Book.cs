using System;

namespace HomeWork.Model
{
    internal class Book
    {
        public string Name { get; set; }
        public int Id { get; set; }

        public string Author { get; set; }

        public DateTime YearCreation;

        //public Image Cover { get; set; }
        public string ISBN { get; set; }

        public string Description { get; set; }

    }
}