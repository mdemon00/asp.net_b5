﻿using DataImporter.Importing.Services;
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
        private IColumnService _columnService;
        private IHttpContextAccessor _httpContextAccessor;
        private IMapper _mapper;
        private ILifetimeScope _scope;

        public ContactListModel()
        {
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _contactService = _scope.Resolve<IContactService>();
            _columnService = _scope.Resolve<IColumnService>();
            _httpContextAccessor = _scope.Resolve<IHttpContextAccessor>();
            _mapper = _scope.Resolve<IMapper>();
        }

        public ContactListModel(IContactService contactService, IColumnService columnService, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _columnService = columnService;
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
                string.IsNullOrWhiteSpace(tableModel.GroupName) ? null : tableModel.GroupName
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
            var _columns = new List<Column>() { };

            try
            {
                _columns = _columnService.GetAllColumns(string.IsNullOrWhiteSpace(tableModel.GroupName) ? null : tableModel.GroupName);
            }
            catch
            {
                return _columns;
            }

            return _columns;
        }

    }
}