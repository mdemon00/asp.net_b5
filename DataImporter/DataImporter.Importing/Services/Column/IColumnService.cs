using DataImporter.Importing.BusinessObjects;
using System.Collections.Generic;


namespace DataImporter.Importing.Services
{
    public interface IColumnService
    {
        IList<Column> GetAllColumns();
        List<Column> GetAllColumns(int groupId);
        void CreateColumn(Column column);
        //(IList<Column> records, int total, int totalDisplay) GetColumns(int pageIndex, int pageSize,
        //    string searchText, string sortText);
        Column GetColumn(int id);
        Column GetColumn(string name);
        void UpdateColumn(Column column);
        void DeleteColumn(int id);
    }
}
