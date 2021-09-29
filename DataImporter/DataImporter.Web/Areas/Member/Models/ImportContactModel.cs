﻿using Autofac;
using AutoMapper;
using DataImporter.Importing.BusinessObjects;
using DataImporter.Importing.Services;
using DataImporter.Web;
using System;
using System.ComponentModel.DataAnnotations;

namespace DataImporter.Areas.Member.Models
{

    public class ImportContactModel
    {
        [Required, MaxLength(200, ErrorMessage = "GroupName should be less than 200 charcaters")]
        public string GroupName { get; set; }

        [Required, MaxLength(200, ErrorMessage = "FileName should be less than 200 charcaters")]

        public string FileName { get; set; }

        private IExcelService _excelService;
        private IHistoryService _historyService;
        private IMapper _mapper;
        private ILifetimeScope _scope;

        public ImportContactModel()
        {
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _excelService = _scope.Resolve<IExcelService>();
            _historyService = _scope.Resolve<IHistoryService>();
            _mapper = _scope.Resolve<IMapper>();
        }

        public ImportContactModel(IExcelService excelService, IHistoryService historyService)
        {
            _excelService = excelService;
            _historyService = historyService;
        }
        internal void Import()
        {
            var history = new History
            {
                FileName = FileName,
                GroupName = GroupName
            };

            _historyService.CreateHistory(history);
            //_excelService.ImportSheet(fullDirectoryAddress, fileName, gorupName);
        }
    }
}
