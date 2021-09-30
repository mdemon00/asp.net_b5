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
        }

        public ExportContactModel(IExcelService contactService, IOptions<WebSettingsModel> settings)
        {
            _excelService = contactService;
            _settings = settings.Value;
        }
        internal void Export()
        {
            // only for one group name. need to implement for multiple
            _excelService.ExportSheet(_settings.Download_Location, GroupNames[0]);
        }
    }
}
