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
using System;

namespace DataImporter.Areas.Member.Models
{
    public class ContactListModel
    {
        private IExcelService _excelService;
        private IColumnService _columnService;
        private IHttpContextAccessor _httpContextAccessor;
        private IMapper _mapper;
        private ILifetimeScope _scope;
        private IGroupService _groupService;

        public ContactListModel()
        {
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _excelService = _scope.Resolve<IExcelService>();
            _columnService = _scope.Resolve<IColumnService>();
            _httpContextAccessor = _scope.Resolve<IHttpContextAccessor>();
            _mapper = _scope.Resolve<IMapper>();
            _groupService = _scope.Resolve<IGroupService>();
        }

        public ContactListModel(IExcelService contactService, IColumnService columnService,
            IHttpContextAccessor httpContextAccessor, IMapper mapper, IGroupService groupService)
        {
            _columnService = columnService;
            _excelService = contactService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _groupService = groupService;
        }

        internal object GetContacts(DataTablesAjaxRequestModel tableModel)
        {
            var group = _groupService.GetGroup(tableModel.GroupName);

            var data = _excelService.GetSheets(
                tableModel.PageIndex,
                tableModel.PageSize,
                tableModel.SearchText,
                tableModel.GetSortText(_columnService.GetAllColumns(group == null ? 0 : group.Id).Select(i => i.Name.ToString()).ToArray()),
                group == null ? 0 : group.Id
                );

            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = data.records.ToArray()
            };

        }

        public IList<Column> GetColums(DataTablesAjaxRequestModel tableModel)
        {
            try
            {
                var group = _groupService.GetGroup(tableModel.GroupName);
                var _columns = _columnService.GetAllColumns(group == null ? 0 : group.Id);
                return _columns;
            }
            catch(Exception ex)
            {
                return new List<Column>() { };
            }
        }
    }
}