using DataImporter.Importing.Services;
using System.Linq;
using Autofac;
using Microsoft.AspNetCore.Http;
using DataImporter.Common.Utilities;
using DataImporter.Web;
using DataImporter.Importing.BusinessObjects;
using System.Collections.Generic;
using AutoMapper;
using System.Reflection;

namespace DataImporter.Areas.Member.Models
{
    public class ContactListModel
    {
        private IContactService _contactService;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        public ContactListModel()
        {
            _contactService = Startup.AutofacContainer.Resolve<IContactService>();
            _httpContextAccessor = Startup.AutofacContainer.Resolve<IHttpContextAccessor>();
            _mapper = Startup.AutofacContainer.Resolve<IMapper>();
        }

        public ContactListModel(IContactService contactService, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _contactService = contactService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        internal object GetContacts(DataTablesAjaxRequestModel tableModel)
        {
            var data = _contactService.GetContacts(
                tableModel.PageIndex,
                tableModel.PageSize,
                tableModel.SearchText,
                tableModel.GetSortText(new string[] { "Timestamp", "Name" }),
                "Attendence"
                );

            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = data.records.ToArray()
            };

        }

        public List<Column> GetColums()
        {
            var _columns = new List<Column>()
            {
                 new Column { Id = 0, GroupId = 0, Name="Manas"},
                 new Column { Id = 0, GroupId = 0, Name="Tester"}
            };

            return _columns;
        }

    }
}