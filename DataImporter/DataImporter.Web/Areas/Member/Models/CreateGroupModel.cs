using DataImporter.Importing.Services;
using Autofac;
using DataImporter.Importing.BusinessObjects;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using DataImporter.Web;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System;
using Microsoft.Extensions.Logging;

namespace DataImporter.Areas.Member.Models
{
    public class CreateGroupModel
    {
        [Required, MaxLength(200, ErrorMessage = "Name should be less than 200 charcaters")]
        public string Name { get; set; }

        private IGroupService _groupService;
        private IMapper _mapper;
        private ILifetimeScope _scope;
        private IHttpContextAccessor _httpContextAccessor;
        public CreateGroupModel()
        {
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _groupService = _scope.Resolve<IGroupService>();
            _mapper = _scope.Resolve<IMapper>();
            _httpContextAccessor = _scope.Resolve<IHttpContextAccessor>();
        }

        internal void CreateGroup()
        {
            var group = new Group
            {
                Name = Name
            };

            _groupService.CreateGroup(group);
        }
    }
}
