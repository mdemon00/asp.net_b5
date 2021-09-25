using DataImporter.Importing.Services;
using System.Linq;
using Autofac;
using Microsoft.AspNetCore.Http;
using DataImporter.Common.Utilities;
using DataImporter.Web;
using System.Collections.Generic;
using DataImporter.Importing.BusinessObjects;
using AutoMapper;
using System.Security.Claims;

namespace DataImporter.Areas.Member.Models
{
    public class GroupListModel
    {
        private IGroupService _groupService;
        private IHttpContextAccessor _httpContextAccessor;
        private IMapper _mapper;
        private ILifetimeScope _scope;

        public GroupListModel()
        {

        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _groupService = _scope.Resolve<IGroupService>();
            _httpContextAccessor = _scope.Resolve<IHttpContextAccessor>();
            _mapper = _scope.Resolve<IMapper>();
        }

        public GroupListModel(IGroupService groupService, IHttpContextAccessor httpContextAccessor)
        {
            _groupService = groupService;
            _httpContextAccessor = httpContextAccessor;
        }

        internal object GetGroups(DataTablesAjaxRequestModel tableModel)
        {
            var data = _groupService.GetGroups(
                tableModel.PageIndex,
                tableModel.PageSize,
                tableModel.SearchText,
                tableModel.GetSortText(new string[] { "Name" }));

            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = (from record in data.records
                        select new string[]
                        {
                                record.Name,
                                record.Id.ToString()
                        }
                    ).ToArray()
            };
        }

        internal IList<Group> GetAllGroups()
        {
            return _groupService.GetAllGroups();
        }

        internal void Delete(int id)
        {
            _groupService.DeleteGroup(id);
        }
    }
}
