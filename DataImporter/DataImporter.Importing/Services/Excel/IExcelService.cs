using DataImporter.Importing.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataImporter.Importing.Services
{
    public interface IExcelService
    {
        DataTable ImportExceltoDatatable(string filePath, string sheetName);
        void ImportSheet(string path, dynamic worksheetName, int groupId = 0);
        void ExportSheet(string path, int groupId = 0);
        void RemoveSheet(string path, dynamic worksheetName);
        (IList<string[]> records, int total, int totalDisplay) GetSheets(int pageIndex, int pageSize,
            string searchText, string sortText, int groupId = 0, bool export = false);
    }
}
