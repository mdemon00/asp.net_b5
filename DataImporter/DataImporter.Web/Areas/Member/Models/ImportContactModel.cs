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

        private IContactService _contactService;
        private IMapper _mapper;
        private ILifetimeScope _scope;

        public ImportContactModel()
        {
            _contactService = Startup.AutofacContainer.Resolve<IContactService>();
            _mapper = Startup.AutofacContainer.Resolve<IMapper>();
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _contactService = _scope.Resolve<IContactService>();
            _mapper = _scope.Resolve<IMapper>();
        }

        public ImportContactModel(IContactService contactService)
        {
            _contactService = contactService;
        }
        internal void Import(string fullDirectoryAddress, string fileName, string gorupName)
        {

            //_contactService.ImportSheet(@"C:\Users\John\Documents\AspAttendence.xlsx", "AspAttendence","Attendence");
            _contactService.ImportSheet(fullDirectoryAddress, fileName, gorupName);
        }
    }
}
