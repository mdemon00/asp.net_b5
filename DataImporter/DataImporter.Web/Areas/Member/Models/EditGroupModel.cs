using DataImporter.Importing.Services;
using System;
using Autofac;
using DataImporter.Importing.BusinessObjects;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using DataImporter.Web;

namespace DataImporter.Areas.Member.Models
{
    public class EditGroupModel
    {
        [Required, Range(1, 50000)]
        public int Id { get; set; }

        [Required, MaxLength(200, ErrorMessage = "Name should be less than 200 charcaters")]
        public string Name { get; set; }

        private readonly IGroupService _groupService;
        private readonly IMapper _mapper;

        public EditGroupModel()
        {
            _groupService = Startup.AutofacContainer.Resolve<IGroupService>();
            _mapper = Startup.AutofacContainer.Resolve<IMapper>();
        }

        public void LoadModelData(int id)
        {
            var group = _groupService.GetGroup(id);

            Name = group.Name;
            Id = group.Id;
        }

        internal void Update()
        {
            var group = new Group
            {
                Id = Id,
                Name = Name
            };
            _groupService.UpdateGroup(group);
        }
    }
}
