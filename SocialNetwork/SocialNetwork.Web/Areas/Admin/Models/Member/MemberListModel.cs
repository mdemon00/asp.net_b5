using Autofac;
using SocialNetwork.Registering.Services;
using SocialNetwork.Web.Models;
using System.Linq;

namespace SocialNetwork.Web.Areas.Admin.Models
{
    public class MemberListModel
    {
        private readonly IMemberService _memberService;
        public MemberListModel()
        {
            _memberService = Startup.AutofacContainer.Resolve<IMemberService>();
        }

        public MemberListModel(IMemberService memberService)
        {
            _memberService = memberService;
        }

        internal object GetMembers(DataTablesAjaxRequestModel tableModel)
        {
            var data = _memberService.GetMembers(tableModel.PageIndex,
                tableModel.PageSize,
                tableModel.SearchText,
                tableModel.GetSortText(new string[] { "Name", "DateofBirth", "Address" }));

            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = (from record in data.records
                        select new string[]
                        {
                                record.Name,
                                record.DateofBirth.ToString(),
                                record.Address.ToString(),
                                record.Id.ToString()
                        }
                    ).ToArray()
            };
        }

        internal void Delete(int id)
        {
            _memberService.DeleteMember(id);
        }
    }
}
