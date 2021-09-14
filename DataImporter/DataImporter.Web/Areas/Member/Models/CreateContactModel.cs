using Autofac;
using AutoMapper;
using DataImporter.Importing.Services;
using DataImporter.Web;
using System;

namespace DataImporter.Areas.Member.Models
{
    public class CreateContactModel
    {
        private readonly IContactService _contactService;
        private readonly IMapper _mapper;

        public CreateContactModel()
        {
            _contactService = Startup.AutofacContainer.Resolve<IContactService>();
            _mapper = Startup.AutofacContainer.Resolve<IMapper>();
        }

        public CreateContactModel(IContactService contactService)
        {
            _contactService = contactService;
        }
        internal void Create()
        {

            _contactService.ImportSheet(@"C:\Users\John\Documents\AspAttendence.xlsx", "AspAttendence","Attendence");
        }
    }
}
