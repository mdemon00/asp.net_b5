using LibraryManagementSystem.Reading.BuisnessObjects;
using System.Collections.Generic;

namespace LibraryManagementSystem.Reading.Services
{
    public interface IAuthorService
    {
        void CreateAuthor(Author author);
        (IList<Author> records, int total, int totalDisplay) GetAuthors(int pageIndex, int pageSize,
    string searchText, string sortText);
        Author GetAuthor(int id);
        void UpdateAuthor(Author author);
        void DeleteAuthor(int id);
    }
}
