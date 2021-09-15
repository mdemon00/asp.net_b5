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
        public List<Column> _columns;
        public ContactListModel()
        {
            _contactService = Startup.AutofacContainer.Resolve<IContactService>();
            _httpContextAccessor = Startup.AutofacContainer.Resolve<IHttpContextAccessor>();
            _mapper = Startup.AutofacContainer.Resolve<IMapper>();
            _columns = _contactService.GetColums();
        }

        public ContactListModel(IContactService contactService, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _contactService = contactService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _columns = _contactService.GetColums();
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

            var cellsGroupList = data.records
            .GroupBy(x => x.RowId)
            .Select(grp => grp.ToList()).ToList();

            var masterArray = new List<string[]>();
            foreach(var cellGroup in cellsGroupList)
            {
                var temp = new List<string>();
                foreach (var cell in cellGroup)
                {
                    foreach (PropertyInfo prop in cell.GetType().GetProperties())
                    {
                        if (prop.Name != "Id" && prop.Name != "RowId")
                        {
                            if (prop.PropertyType.IsPrimitive || prop.PropertyType == typeof(string))
                                temp.Add(prop.GetValue(cell).ToString());
                        }

                    }
                }
                temp.Add(cellGroup.First().RowId.ToString());

                masterArray.Add(temp.ToArray());

            }

            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = masterArray.ToArray()
            };

        }

    }
}
