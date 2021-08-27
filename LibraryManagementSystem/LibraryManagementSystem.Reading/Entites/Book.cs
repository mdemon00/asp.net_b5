using LibraryManagementSystem.Data;
using System.Collections.Generic;

namespace LibraryManagementSystem.Reading.Entites
{
    public class Book : IEntity<int>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Barcode { get; set; }
        public List<Author> Authors { get; set; }
        public double Price { get; set; }
        public List<BookAuthors> RentedBooks { get; set; }
    }
}
