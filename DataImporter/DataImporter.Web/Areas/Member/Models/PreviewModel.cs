using Autofac;
using AutoMapper;
using DataImporter.Importing.Services;
using DataImporter.Web;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace DataImporter.Areas.Member.Models
{
    public class PreviewModel
    {
        [Required, MaxLength(200, ErrorMessage = "FileName should be less than 200 charcaters")]

        public string FileName { get; set; }

        private IExcelService _excelService;
        private IMapper _mapper;
        private ILifetimeScope _scope;

        public PreviewModel()
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

        public PreviewModel(IExcelService contactService)
        {
            _excelService = contactService;
        }
        internal DataTable GetPreview(string filePath, string sheetName)
        {
            return _excelService.ImportExceltoDatatable(filePath, sheetName);
        }
    }
}
