using Autofac;
using AutoMapper;
using DataImporter.Importing.Services;
using DataImporter.Web;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace DataImporter.Areas.Member.Models
{
    public class DashboardModel
    {
        [Required, MaxLength(200, ErrorMessage = "FileName should be less than 200 charcaters")]

        public int GroupCount { get; set; }
        public int PendingCount { get; set; }
        public int ImportCount { get; set; }
        public int ExportCount { get; set; }

        private IHistoryService _historyService;
        private IGroupService _groupService;

        public DashboardModel()
        {
        } 
        
        public DashboardModel(IHistoryService historyService, IGroupService groupService)
        {
            _historyService = historyService;
            _groupService = groupService;
        }

        public void GetGroupCount()
        {
            GroupCount = _groupService.GetGroupCount();
        }     
        public void GetPendingCount()
        {
            PendingCount = _historyService.GetPendingTaskCount();
        }   
        public void GetImportCount()
        {
            ImportCount = _historyService.GetImportedCount();
        }    
        public void GetExportCount()
        {
            ExportCount = _historyService.GetExportedCount();
        }    
    }
}
