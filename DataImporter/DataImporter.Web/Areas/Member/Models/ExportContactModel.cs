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

        private IContactService _contactService;
        private IMapper _mapper;
        private ILifetimeScope _scope;

        public ExportContactModel()
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

        public ExportContactModel(IContactService contactService)
        {
            _contactService = contactService;
        }
        internal void Export()
        {
            
            _contactService.ExportSheet(GroupNames);
        }
    }
}
