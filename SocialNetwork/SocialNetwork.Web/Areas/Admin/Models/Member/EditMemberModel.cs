using Autofac;
using SocialNetwork.Registering.BuisnessObjects;
using SocialNetwork.Registering.Services;
using System;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Web.Areas.Admin.Models
{
    public class EditMemberModel
    {
        [Required, Range(1, 50000000)]
        public int? Id { get; set; }

        [Required, MaxLength(50, ErrorMessage = "Name should be less than 50 charcaters")]
        public string Name { get; set; }
        public DateTime DateofBirth { get; set; }

        [Required, MaxLength(50, ErrorMessage = "Address should be less than 50 charcaters")]
        public string Address { get; set; }

        private readonly IMemberService _memberService;
        public EditMemberModel()
        {
            _memberService = Startup.AutofacContainer.Resolve<IMemberService>();
        }

        public EditMemberModel(IMemberService memberService)
        {
            _memberService = memberService;
        }

        public void LoadModelData(int id)
        {
            var member = _memberService.GetMember(id);
            Name = member.Name;
            DateofBirth = member.DateofBirth;
            Address = member?.Address;
        }

        internal void Update()
        {
            var member = new Member
            {
                Id = Id.HasValue ? Id.Value : 0,
                Name = Name,
                DateofBirth = DateofBirth,
                Address = Address
            };
            _memberService.UpdateMember(member);
        }
    }
}
