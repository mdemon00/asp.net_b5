using Autofac;
using AutoMapper;
using DataImporter.Importing.BusinessObjects;
using DataImporter.Importing.Services;
using DataImporter.Web;
using DataImporter.Web.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace DataImporter.Areas.Member.Models
{
    public class ExportContactModel
    {
        //[Required, MaxLength(200, ErrorMessage = "GroupName should be less than 200 charcaters")]
        public List<String> GroupNames { get; set; }
        public string FileName { get; set; }
        public string Email { get; set; }

        private IExcelService _excelService;
        private IMapper _mapper;
        private ILifetimeScope _scope;
        private IGroupService _groupService;
        private IHistoryService _historyService;
        private WebSettingsModel _settings;

        public ExportContactModel()
        {
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _excelService = _scope.Resolve<IExcelService>();
            _mapper = _scope.Resolve<IMapper>();
            _settings = _scope.Resolve<WebSettingsModel>();
            _groupService = _scope.Resolve<IGroupService>();
            _historyService = _scope.Resolve<IHistoryService>();
        }

        public ExportContactModel(IExcelService contactService, IOptions<WebSettingsModel> settings, IGroupService groupService, IHistoryService historyService)
        {
            _excelService = contactService;
            _settings = settings.Value;
            _groupService = groupService;
            _historyService = historyService;
        }
        internal void Export()
        {
            if(GroupNames.Count < 1)
                throw new InvalidOperationException("No Group found");

            // only for one group name. need to implement for multiple
            var group = _groupService.GetGroup(GroupNames[0]);

            if(group == null)
                throw new InvalidOperationException("No Group found");

            var history = new History
            {
                FileName = group.Name + ".xlsx",
                GroupId =  group.Id,
                ProcessType = "Export",
                Status = "Pending",
                CreatedDate = DateTime.Now,
                Email = Email
            };

            _historyService.CreateHistory(history);
        }
    }
}
