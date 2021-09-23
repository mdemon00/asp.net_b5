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

        private readonly IContactService _contactService;
        private readonly IMapper _mapper;

        public ImportContactModel()
        {
            _contactService = Startup.AutofacContainer.Resolve<IContactService>();
            _mapper = Startup.AutofacContainer.Resolve<IMapper>();
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
