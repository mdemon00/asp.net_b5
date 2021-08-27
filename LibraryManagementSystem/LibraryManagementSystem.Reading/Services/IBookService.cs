using LibraryManagementSystem.Reading.BuisnessObjects;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Reading.Services
{
    public interface IBookService
    {
        void CreateBook(Book book);
        (IList<Book> records, int total, int totalDisplay) GetBooks(int pageIndex, int pageSize,
    string searchText, string sortText);
        Book GetBook(int id);
        void UpdateBook(Book book);
        void DeleteBook(int id);
    }
}
