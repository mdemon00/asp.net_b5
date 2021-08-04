using SocialNetwork.Registering.BuisnessObjects;
using SocialNetwork.Registering.Exceptions;
using SocialNetwork.Registering.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SocialNetwork.Registering.Services
{
    public class MemberService : IMemberService
    {
        private readonly IRegisteringUnitOfWork _registeringUnitOfWork;

        public MemberService(IRegisteringUnitOfWork registeringUnitOfWork)
        {
            _registeringUnitOfWork = registeringUnitOfWork;
        }

        public void CreateMember(Member member)
        {
            if (member == null)
                throw new InvalidParameterException("Member was not provided");

            _registeringUnitOfWork.Members.Add(new Entites.Member
            {
                Name = member.Name,
                DateofBirth = member.DateofBirth,
                Address = member.Address
            });

            _registeringUnitOfWork.Save();
        }

        public void DeleteMember(int id)
        {
            _registeringUnitOfWork.Members.Remove(id);
            _registeringUnitOfWork.Save();
        }

        public Member GetMember(int id)
        {
            var member = _registeringUnitOfWork.Members.GetById(id);

            if (member == null) return null;

            return new Member
            {
                Id = member.Id,
                Name = member.Name,
                DateofBirth = member.DateofBirth,
                Address = member.Address
            };
        }

        public (IList<Member> records, int total, int totalDisplay) GetMembers(int pageIndex, int pageSize, string searchText, string sortText)
        {

            int value = 0;
            int.TryParse(searchText, out value); // determine whether a string represents a numeric value

            var memberData = _registeringUnitOfWork.Members.GetDynamic(string.IsNullOrWhiteSpace(searchText) ? null : x => x.Name.Contains(searchText),
                sortText, string.Empty, pageIndex, pageSize);

            var resultData = (from member in memberData.data
                              select new Member
                              {
                                  Id = member.Id,
                                  Name = member.Name,
                                  DateofBirth = member.DateofBirth,
                                  Address = member.Address
                              }).ToList();
            return (resultData, memberData.total, memberData.totalDisplay);
        }

        public void UpdateMember(Member member)
        {
            if (member == null)
                throw new InvalidOperationException("Member is missing");

            var memberEntity = _registeringUnitOfWork.Members.GetById(member.Id);

            if (memberEntity != null)
            {
                memberEntity.Name = member.Name;
                memberEntity.DateofBirth = member.DateofBirth;
                memberEntity.Address = member.Address;

                _registeringUnitOfWork.Save();
            }
            else
                throw new InvalidOperationException("Couldn't find member");
        }
    }
}
