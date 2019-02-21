using System;
using System.Data.Entity;

namespace Fisher.Bookstore.Models
{
    public class Books
    {
        public int Id {get; set;}

        public string Title {get; set;}

        public string Author {get; set;}

        public string ISBN {get; set;}

        public DateTime publiccationDate {get; set;}

    }
}