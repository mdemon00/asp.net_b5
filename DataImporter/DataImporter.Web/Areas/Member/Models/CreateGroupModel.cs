using DataImporter.Importing.Services;
using Autofac;
using DataImporter.Importing.BusinessObjects;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using DataImporter.Web;

namespace DataImporter.Areas.Member.Models
{
    public class CreateGroupModel
    {
        [Required, MaxLength(200, ErrorMessage = "Name should be less than 200 charcaters")]
        public string Name { get; set; }

        private readonly IGroupService _groupService;
        private readonly IMapper _mapper;

        public CreateGroupModel()
        {
            _groupService = Startup.AutofacContainer.Resolve<IGroupService>();
            _mapper = Startup.AutofacContainer.Resolve<IMapper>();
        }

        public CreateGroupModel(IGroupService groupService)
        {
            _groupService = groupService;
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
