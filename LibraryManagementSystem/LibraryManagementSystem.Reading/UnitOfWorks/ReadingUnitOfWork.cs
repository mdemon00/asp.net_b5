using LibraryManagementSystem.Data;
using LibraryManagementSystem.Reading.Contexts;
using LibraryManagementSystem.Reading.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Reading.UnitOfWorks
{
    public class ReadingUnitOfWork : UnitOfWork, IReadingUnitOfWork
    {
        public IBookRepository Books { get; private set; }
        public IAuthorRepository Authors { get; private set; }

        public ReadingUnitOfWork(IReadingContext context, IBookRepository books, IAuthorRepository authors) : base((DbContext)context)
        {
            Books = books;
            Authors = authors;
        }
    }
}


