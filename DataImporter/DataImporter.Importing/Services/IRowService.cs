using DataImporter.Importing.BusinessObjects;
using System.Collections.Generic;


namespace DataImporter.Importing.Services
{
    public interface IRowService
    {
        IList<Row> GetAllRows();
        Entities.Row CreateRow(Row row);
        //(IList<Row> records, int total, int totalDisplay) GetRows(int pageIndex, int pageSize,
        //    string searchText, string sortText);
        Row GetRow(int id);
        Row GetRow(string groupId);
        void UpdateRow(Row row);
        void DeleteRow(int id);
    }
}
