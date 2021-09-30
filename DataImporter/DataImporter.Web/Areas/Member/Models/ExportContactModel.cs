using Autofac;
using AutoMapper;
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
        public string fileName { get; set; }

        private IExcelService _excelService;
        private IMapper _mapper;
        private ILifetimeScope _scope;
        private IGroupService _groupService;

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
        }

        public ExportContactModel(IExcelService contactService, IOptions<WebSettingsModel> settings, IGroupService groupService)
        {
            _excelService = contactService;
            _settings = settings.Value;
            _groupService = groupService;
        }
        internal void Export()
        {
            // only for one group name. need to implement for multiple
            var group = _groupService.GetGroup(GroupNames[0]);

            _excelService.ExportSheet(_settings.Download_Location, group == null ? 0 : group.Id);
        }
    }
}
