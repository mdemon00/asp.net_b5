using Autofac;
using SocialNetwork.Registering.BuisnessObjects;
using SocialNetwork.Registering.Services;
using System;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Web.Areas.Admin.Models
{
    public class CreateMemberModel
    {
        [Required, MaxLength(50, ErrorMessage = "Name should be less than 50 charcaters")]
        public string Name { get; set; }
        public DateTime DateofBirth { get; set; }

        [Required, MaxLength(50, ErrorMessage = "Address should be less than 50 charcaters")]
        public string Address { get; set; }

        private readonly IMemberService _memberService;
        public CreateMemberModel()
        {
            _memberService = Startup.AutofacContainer.Resolve<IMemberService>();
        }

        public CreateMemberModel(IMemberService memberService)
        {
            _memberService = memberService;
        }

        internal void CreateMember()
        {
            var member = new Member
            {
                Name = Name,
                DateofBirth = DateofBirth,
                Address = Address
            };

            _memberService.CreateMember(member);
        }
    }
}
