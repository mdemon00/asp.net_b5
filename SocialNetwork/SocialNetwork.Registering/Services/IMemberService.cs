using SocialNetwork.Registering.BuisnessObjects;
using System.Collections.Generic;

namespace SocialNetwork.Registering.Services
{
    public interface IMemberService
    {
        (IList<Member> records, int total, int totalDisplay) GetMembers(int pageIndex, int pageSize, string searchText, string sortText);
        void CreateMember(Member member);
        Member GetMember(int id);
        void UpdateMember(Member member);
        void DeleteMember(int id);
    }
}
