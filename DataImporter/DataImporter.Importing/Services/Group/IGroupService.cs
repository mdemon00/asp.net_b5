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
        Group GetGroup(int id, bool fromWorkerService = false);
        Group GetGroup(string name, bool fromWorkerService = false);
        void UpdateGroup(Group group, bool fromWorkerService = false);
        void DeleteGroup(int id);
    }
}
