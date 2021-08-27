using LibraryManagementSystem.Data;
using LibraryManagementSystem.Reading.Contexts;
using LibraryManagementSystem.Reading.Entites;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Reading.Repositories
{
    public class AuthorRepository : Repository<Author, int>, IAuthorRepository
    {
        public AuthorRepository(IReadingContext context) : base((DbContext)context)
        {

        }
    }
}
