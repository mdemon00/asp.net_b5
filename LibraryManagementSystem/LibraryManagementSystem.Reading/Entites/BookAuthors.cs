﻿namespace LibraryManagementSystem.Reading.Entites
{
    public class BookAuthors
    {
        public int BookId { get; set; }
        public Book Book { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
