using Autofac;
using AutoMapper;
using DataImporter.Importing.Services;
using DataImporter.Web;
using System;
using System.ComponentModel.DataAnnotations;

namespace DataImporter.Areas.Member.Models
{

    public class ImportContactModel
    {
        [Required, MaxLength(200, ErrorMessage = "GroupName should be less than 200 charcaters")]
        public string GroupName { get; set; }

        [Required, MaxLength(200, ErrorMessage = "FileName should be less than 200 charcaters")]

        public string FileName { get; set; }

        private IExcelService _excelService;
        private IMapper _mapper;
        private ILifetimeScope _scope;

        public ImportContactModel()
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

        public ImportContactModel(IExcelService contactService)
        {
            _excelService = contactService;
        }
        internal void Import(string fullDirectoryAddress, string fileName, string gorupName)
        {

            _excelService.ImportSheet(fullDirectoryAddress, fileName, gorupName);
        }
    }
}
