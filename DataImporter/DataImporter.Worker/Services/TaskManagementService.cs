using DataImporter.Importing.Services;
using DataImporter.Importing.Services.Mail;
using DataImporter.Worker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImporter.Worker.Services
{
    public class TaskManagementService : ITaskManagementService
    {
        private IHistoryService _historyService;
        private IExcelService _excelService;
        private IMailService _mailService;

        private WorkerSettingsModel _settings;
        public TaskManagementService(IHistoryService historyService, IExcelService excelService,
            IOptions<WorkerSettingsModel> settings, IMailService mailService)
        {
            _historyService = historyService;
            _settings = settings.Value;
            _excelService = excelService;
            _mailService = mailService;
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
                            history.Status = "Processing";
                            _historyService.UpdateHistory(history);

                            _excelService.ImportSheet(_settings.Upload_Location, history.FileName, history.GroupId);

                            history.Status = "Completed";
                            _historyService.UpdateHistory(history);

                            _excelService.RemoveSheet(_settings.Upload_Location, history.FileName);
                        }
                        catch(Exception ex)
                        {
                            history.Status = "Failed";
                            _historyService.UpdateHistory(history);

                            throw new InvalidOperationException("Error " + ex);
                        }
                    }
                    else if (history.ProcessType == "Export")
                    {
                        try
                        {
                            history.Status = "Processing";
                            _historyService.UpdateHistory(history);

                            _excelService.ExportSheet(_settings.Download_Location, history.GroupId);

                            history.Status = "Completed";
                            _historyService.UpdateHistory(history);
                        }
                        catch (Exception ex)
                        {
                            history.Status = "Failed";
                            _historyService.UpdateHistory(history);

                            throw new InvalidOperationException("Error " + ex);
                        }

                        if (history.EmailSent == 0 && !string.IsNullOrEmpty(history.Email))
                        {
                            var filesPath = new List<string>()
                            {
                                Path.Combine(_settings.Download_Location, history.FileName)
                            };

                            var request = new MailRequest()
                            {
                                ToEmail = history.Email,
                                Subject = "Hello there..",
                                Body = "Mail from DataImporter!",
                                FilesPath = filesPath
                            };

                            try
                            {
                                _mailService.SendEmailAsync(request).Wait();

                                history.EmailSent = 1;

                                _historyService.UpdateHistory(history);
                            }
                            catch (Exception ex)
                            {
                                history.EmailSent = 2;

                                throw new InvalidOperationException("Error " + ex);
                            }
                        }
                    }
                }
            }
        }
    }
}
