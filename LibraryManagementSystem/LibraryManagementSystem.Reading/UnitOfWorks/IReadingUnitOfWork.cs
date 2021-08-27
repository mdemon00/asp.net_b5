using LibraryManagementSystem.Data;
using LibraryManagementSystem.Reading.Repositories;

namespace LibraryManagementSystem.Reading.UnitOfWorks
{
    public interface IReadingUnitOfWork : IUnitOfWork
    {
        IBookRepository Books { get; }
        IAuthorRepository Authors { get; }
    }
}
