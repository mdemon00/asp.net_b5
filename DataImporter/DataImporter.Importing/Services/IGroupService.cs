using DataImporter.Importing.BusinessObjects;
using System.Collections.Generic;


namespace DataImporter.Importing.Services
{
    public interface IGroupService
    {
        IList<Group> GetAllGroups();
        void CreateGroup(Group group);
        (IList<Group> records, int total, int totalDisplay) GetGroups(int pageIndex, int pageSize,
            string searchText, string sortText);
        Group GetGroup(int id);
        Group GetGroup(string name);
        void UpdateGroup(Group group);
        void DeleteGroup(int id);
    }
}
