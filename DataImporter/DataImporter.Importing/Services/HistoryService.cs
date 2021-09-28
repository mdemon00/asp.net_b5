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
}
