using DataImporter.Importing.BusinessObjects;
using System.Collections.Generic;

namespace DataImporter.Importing.Services
{
    public interface IContactService
    {
        void ImportSheet(string path, dynamic worksheetName, string groupName);
        (IList<Cell> records, int total, int totalDisplay) GetContacts(int pageIndex, int pageSize,
            string searchText, string sortText, string groupName);
    }
}
