using DataImporter.Importing.BusinessObjects;
using System.Collections.Generic;


namespace DataImporter.Importing.Services
{
    public interface IHistoryService
    {
        void CreateHistory(History history);
        (IList<History> records, int total, int totalDisplay) GetHistories(int pageIndex, int pageSize,
    string searchText, string sortText);
        IList<History> GetPendingHistory();
        void UpdateHistory(History history);
        int GetPendingTaskCount(); 
        int GetImportedCount(); 
        int GetExportedCount(); 

    }
}
