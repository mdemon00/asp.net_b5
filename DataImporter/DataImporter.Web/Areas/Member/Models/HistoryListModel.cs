﻿using Autofac;
using AutoMapper;
using DataImporter.Common.Utilities;
using DataImporter.Importing.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace DataImporter.Areas.Member.Models
{
    public class HistoryListModel
    {
        private IHistoryService _historyService;
        private IHttpContextAccessor _httpContextAccessor;
        private IMapper _mapper;
        private ILifetimeScope _scope;
        public HistoryListModel()
        {

        }
        public HistoryListModel(IHistoryService historyService, IHttpContextAccessor httpContextAccessor)
        {
            _historyService = historyService;
            _httpContextAccessor = httpContextAccessor;
        }
        internal object GetHistories(DataTablesAjaxRequestModel tableModel)
        {
            var data = _historyService.GetHistories(
                tableModel.PageIndex,
                tableModel.PageSize,
                tableModel.SearchText,
                tableModel.GetSortText(new string[] { "CreatedDate", "FileName", "GroupName", "ProcessType", "Status" }));

            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = (from record in data.records
                        select new string[]
                        {
                                record.CreatedDate.ToString(),
                                record.FileName,
                                record.GroupName,
                                record.ProcessType,
                                record.Status
                        }
                    ).ToArray()
            };
        }
    }
}
