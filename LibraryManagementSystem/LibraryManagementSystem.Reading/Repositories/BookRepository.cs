using LibraryManagementSystem.Data;
using LibraryManagementSystem.Reading.Contexts;
using LibraryManagementSystem.Reading.Entites;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Reading.Repositories
{
    public class BookRepository : Repository<Book, int>, IBookRepository
    {
        public BookRepository(IReadingContext context) : base((DbContext)context)
        {

        }
    }
}
