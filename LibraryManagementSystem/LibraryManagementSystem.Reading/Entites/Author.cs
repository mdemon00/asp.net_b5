using LibraryManagementSystem.Data;
using System;
using System.Collections.Generic;

namespace LibraryManagementSystem.Reading.Entites
{
    public class Author : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public List<Book> WrittenBooks { get; set; }
        public Book Book { get; set; }
        public List<BookAuthors> RentedBooks { get; set; }
    }
}
