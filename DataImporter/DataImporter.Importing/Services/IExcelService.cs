using DataImporter.Importing.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataImporter.Importing.Services
{
    public interface IExcelService
    {
        DataTable ImportExceltoDatatable(string filePath, string sheetName);
        void ImportSheet(string path, dynamic worksheetName, string groupName);
        void ExportSheet(string groupName);
        (IList<string[]> records, int total, int totalDisplay) GetSheets(int pageIndex, int pageSize,
            string searchText, string sortText, string groupName, bool export = false);
        List<Column> GetColums();
    }
}
