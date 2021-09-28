using AutoMapper;
using DataImporter.Importing.BusinessObjects;
using DataImporter.Importing.Exceptions;
using DataImporter.Importing.UnitOfWorks;
using DataImporter.Membership.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DataImporter.Importing.Services
{
    public class HistoryService : IHistoryService
    {
        private readonly IImportingUnitOfWork _importingUnitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private IHttpContextAccessor _httpContextAccessor;

        public HistoryService(IImportingUnitOfWork importingUnitOfWork,
            UserManager<ApplicationUser> userManager,
            IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _importingUnitOfWork = importingUnitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public void CreateHistory(History history)
        {
            if (history == null)
                throw new InvalidParameterException("History was not provided");

            if (!IsUserAvailable())
                throw new InvalidParameterException("User not registered");

            if (Guid.TryParse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), out var ApplicationUserId))
                history.ApplicationUserId = ApplicationUserId;
            else
                throw new InvalidParameterException("Something Went Wrong");

            _importingUnitOfWork.Histories.Add(
                _mapper.Map<Entities.History>(history)
            );

            _importingUnitOfWork.Save();
        }

        public (IList<History> records, int total, int totalDisplay) GetHistories(int pageIndex, int pageSize, string searchText, string sortText)
        {
            if (!IsUserAvailable())
                throw new InvalidParameterException("User not registered");

            if (!Guid.TryParse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), out var ApplicationUserId))
                throw new InvalidParameterException("Something Went Wrong");

            var historyData = _importingUnitOfWork.Histories.GetDynamic(
                  !string.IsNullOrWhiteSpace(searchText) ?
                  x => x.ApplicationUserId == ApplicationUserId && x.GroupName.Contains(searchText) :
                  x => x.ApplicationUserId == ApplicationUserId,
                  sortText, string.Empty, pageIndex, pageSize);

            var resultData = (from hr in historyData.data
                              select _mapper.Map<History>(hr)).ToList();

            return (resultData, historyData.total, historyData.totalDisplay);
        }

        public IList<History> GetPendingHistory()
        {
            var historyData = _importingUnitOfWork.Histories.GetDynamic(x => x.Status == "Pending",null,null,false);

            var resultData = (from hr in historyData
                              select _mapper.Map<History>(hr)).ToList();

            return resultData;
        }

        public void UpdateHistory(History history)
        {
            if (history == null)
                throw new InvalidOperationException("History is missing");

            var historyEntity = _importingUnitOfWork.Histories.GetById(history.Id);

            if (historyEntity != null)
            {
                _mapper.Map(history, historyEntity);
                _importingUnitOfWork.Save();
            }
            else
                throw new InvalidOperationException("Couldn't find history");
        }
        private bool IsUserAvailable()
        {
            string userId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);

            return !string.IsNullOrEmpty(userId);
        }
    }
    public class GroupService : IGroupService
    {
        private readonly IImportingUnitOfWork _importingUnitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private IHttpContextAccessor _httpContextAccessor;

        public GroupService(IImportingUnitOfWork importingUnitOfWork,
            UserManager<ApplicationUser> userManager,
            IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _importingUnitOfWork = importingUnitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public IList<Group> GetAllGroups()
        {
            if (!IsUserAvailable())
                throw new InvalidParameterException("User not registered");

            if (!Guid.TryParse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), out var ApplicationUserId))
                throw new InvalidParameterException("Something Went Wrong");

            var groupEntities = _importingUnitOfWork.Groups.GetDynamic(x => x.ApplicationUserId == ApplicationUserId, null, null, false);

            var groups = new List<Group>();

            foreach (var entity in groupEntities)
            {
                var group = _mapper.Map<Group>(entity);
                groups.Add(group);
            }

            return groups;
        }

        public void CreateGroup(Group group)
        {
            if (group == null)
                throw new InvalidParameterException("Group was not provided");

            if (IsNameAlreadyUsed(group.Name))
                throw new DuplicateNameException("Group name already exists");

            if (!IsUserAvailable())
                throw new InvalidParameterException("User not registered");

            if (Guid.TryParse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), out var ApplicationUserId))
                group.ApplicationUserId = ApplicationUserId;
            else
                throw new InvalidParameterException("Something Went Wrong");

            _importingUnitOfWork.Groups.Add(
                _mapper.Map<Entities.Group>(group)
            );

            _importingUnitOfWork.Save();
        }

        private bool IsNameAlreadyUsed(string name) =>
            _importingUnitOfWork.Groups.GetCount(x => x.Name == name) > 0;

        private bool IsNameAlreadyUsed(string name, int id) =>
            _importingUnitOfWork.Groups.GetCount(x => x.Name == name && x.Id != id) > 0;

        private bool IsIdAvaiable(int id) =>
            _importingUnitOfWork.Groups.GetCount(x => x.Id != id) > 0;  
        
        private bool IsGroupBelongsToOwner(Guid applicationUserId, int groupId = 0, string name = null) =>
            _importingUnitOfWork.Groups.GetCount(
                !string.IsNullOrWhiteSpace(name) ?
                x => x.Name == name && x.ApplicationUserId == applicationUserId :
                x => x.Id == groupId && x.ApplicationUserId == applicationUserId
                ) > 0;

        private bool IsUserAvailable()
        {
            string userId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);

            return !string.IsNullOrEmpty(userId);
        }

        public (IList<Group> records, int total, int totalDisplay) GetGroups(int pageIndex, int pageSize,
            string searchText, string sortText)
        {
            if (!IsUserAvailable())
                throw new InvalidParameterException("User not registered");

            if (!Guid.TryParse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), out var ApplicationUserId))
                throw new InvalidParameterException("Something Went Wrong");

            var groupData = _importingUnitOfWork.Groups.GetDynamic(
                  !string.IsNullOrWhiteSpace(searchText) ?
                  x => x.ApplicationUserId == ApplicationUserId && x.Name.Contains(searchText) :
                  x => x.ApplicationUserId == ApplicationUserId,
                  sortText, string.Empty, pageIndex, pageSize);

            var resultData = (from gr in groupData.data
                              select _mapper.Map<Group>(gr)).ToList();

            return (resultData, groupData.total, groupData.totalDisplay);
        }

        public Group GetGroup(int id)
        {
            if (!IsUserAvailable())
                throw new InvalidParameterException("User not registered");

            if (!Guid.TryParse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), out var ApplicationUserId))
                throw new InvalidParameterException("Something Went Wrong");

            if (!IsGroupBelongsToOwner(ApplicationUserId, id))
                throw new InvalidParameterException("Unauthorized Access");

            var group = _importingUnitOfWork.Groups.GetById(id);

            if (group == null) return null;

            return _mapper.Map<Group>(group);
        }

        public Group GetGroup(string name)
        {
            if (!IsUserAvailable())
                throw new InvalidParameterException("User not registered");

            if (!Guid.TryParse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), out var ApplicationUserId))
                throw new InvalidParameterException("Something Went Wrong");

            if (!IsGroupBelongsToOwner(ApplicationUserId, 0, name))
                throw new InvalidParameterException("Unauthorized Access");

            var group = _importingUnitOfWork.Groups.GetDynamic(
                string.IsNullOrWhiteSpace(name) ? null : x => x.Name.Contains(name));

            if (group == null) return null;

            return _mapper.Map<Group>(group);

        }
        public void UpdateGroup(Group group)
        {
            if (group == null)
                throw new InvalidOperationException("Group is missing");

            if (!IsUserAvailable())
                throw new InvalidParameterException("User not registered");

            if (Guid.TryParse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), out var ApplicationUserId))
                group.ApplicationUserId = ApplicationUserId;
            else
                throw new InvalidParameterException("Something Went Wrong");

            if (!IsGroupBelongsToOwner(ApplicationUserId, group.Id))
                throw new InvalidParameterException("Unauthorized Access");

            if (IsNameAlreadyUsed(group.Name, group.Id))
                throw new DuplicateNameException("Group name already used in other group.");

            var groupEntity = _importingUnitOfWork.Groups.GetById(group.Id);

            if (groupEntity != null)
            {
                _mapper.Map(group, groupEntity);
                _importingUnitOfWork.Save();
            }
            else
                throw new InvalidOperationException("Couldn't find group");
        }

        public void DeleteGroup(int id)
        {
            if (!IsIdAvaiable(id))
                throw new DuplicateNameException("Group Id is invalid");

            if (!IsUserAvailable())
                throw new InvalidParameterException("User not registered");

            if (!Guid.TryParse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), out var ApplicationUserId))
                throw new InvalidParameterException("Something Went Wrong");

            if (!IsGroupBelongsToOwner(ApplicationUserId, id))
                throw new InvalidParameterException("Unauthorized Access");

            try
            {
                _importingUnitOfWork.Groups.Remove(id);
                _importingUnitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Couldn't delete group" + ex);
            }

        }
    }
}
