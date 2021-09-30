using Autofac;
using AutoMapper;
using DataImporter.Importing.Services;
using DataImporter.Web;
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

        public ExportContactModel()
        {
            _excelService = Startup.AutofacContainer.Resolve<IExcelService>();
            _mapper = Startup.AutofacContainer.Resolve<IMapper>();
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _excelService = _scope.Resolve<IExcelService>();
            _mapper = _scope.Resolve<IMapper>();
        }

        public ExportContactModel(IExcelService contactService)
        {
            _excelService = contactService;
        }
        internal void Export()
        {
            // only for one group name. need to implement for multiple
            _excelService.ExportSheet(GroupNames[0]);
        }
    }
}
