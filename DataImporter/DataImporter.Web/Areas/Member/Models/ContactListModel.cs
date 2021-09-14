using DataImporter.Importing.Services;
using System.Linq;
using Autofac;
using Microsoft.AspNetCore.Http;
using DataImporter.Common.Utilities;
using DataImporter.Web;

namespace DataImporter.Areas.Member.Models
{
    public class ContactListModel
    {
        private IContactService _contactService;
        private IHttpContextAccessor _httpContextAccessor;

        public ContactListModel()
        {
            _contactService = Startup.AutofacContainer.Resolve<IContactService>();
            _httpContextAccessor = Startup.AutofacContainer.Resolve<IHttpContextAccessor>();
        }

        public ContactListModel(IContactService contactService, IHttpContextAccessor httpContextAccessor)
        {
            _contactService = contactService;
            _httpContextAccessor = httpContextAccessor;
        }

        internal object GetContacts(DataTablesAjaxRequestModel tableModel)
        {
            var data = _contactService.GetContacts(
                tableModel.PageIndex,
                tableModel.PageSize,
                tableModel.SearchText,
                tableModel.GetSortText(new string[] { "Name" }),
                "Atendence"
                );

            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = (from record in data.records
                        select new string[]
                        {
                                record.Data,
                                record.Id.ToString()
                        }
                    ).ToArray()
            };
        }

    }
}
