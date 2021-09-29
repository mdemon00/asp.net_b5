using DataImporter.Importing.Services;
using DataImporter.Worker.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImporter.Worker.Services
{
    public interface ITaskManagementService
    {
        void CompletePendingTask();

    }
    public class TaskManagementService : ITaskManagementService
    {
        private IHistoryService _historyService;
        private IExcelService _excelService;

        private WorkerSettingsModel _settings;
        public TaskManagementService(IHistoryService historyService, IExcelService excelService,
            IOptions<WorkerSettingsModel> settings)
        {
            _historyService = historyService;
            _settings = settings.Value;
            _excelService = excelService;
        }
        public void CompletePendingTask()
        {
            var histories = _historyService.GetPendingHistory();

            if (histories.Count > 0)
            {
                foreach (var history in histories)
                {
                    if (history.ProcessType == "Import")
                    {
                        try
                        {
                            _excelService.ImportSheet(_settings.Upload_Location, history.FileName, history.GroupName);
                        }
                        catch(Exception ex)
                        {
                            throw new InvalidOperationException("Error" + ex);
                        }
                    }
                    else if (history.ProcessType == "Export")
                    {
                        try
                        {

                        }
                        catch (Exception ex)
                        {
                            throw new InvalidOperationException("Error" + ex);
                        }
                    }
                }
            }
        }
    }
}
