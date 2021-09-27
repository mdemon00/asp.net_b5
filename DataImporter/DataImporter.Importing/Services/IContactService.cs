﻿using DataImporter.Importing.BusinessObjects;
using System;
using System.Collections.Generic;

namespace DataImporter.Importing.Services
{
    public interface IContactService
    {
        void ImportSheet(string path, dynamic worksheetName, string groupName);
        void ExportSheet(List<String> groupNames);
        (IList<string[]> records, int total, int totalDisplay) GetContacts(int pageIndex, int pageSize,
            string searchText, string sortText, string groupName, bool export = false);
        List<Column> GetColums();
    }
}
