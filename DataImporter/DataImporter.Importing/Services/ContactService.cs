﻿using AutoMapper;
using ClosedXML.Excel;
using DataImporter.Importing.BusinessObjects;
using DataImporter.Importing.Exceptions;
using DataImporter.Importing.UnitOfWorks;
using System.Collections.Generic;
using System.Linq;

namespace DataImporter.Importing.Services
{
    public class ContactService : IContactService
    {
        private readonly IImportingUnitOfWork _importingUnitOfWork;
        private readonly IMapper _mapper;
        private readonly IGroupService _groupService;
        private readonly IColumnService _columnService;
        private readonly IRowService _rowService;
        private readonly ICellService _cellService;

        public ContactService(IImportingUnitOfWork importingUnitOfWork,
            IMapper mapper, IGroupService groupService, IColumnService columnService, IRowService rowService, ICellService cellService)
        {
            _importingUnitOfWork = importingUnitOfWork;
            _mapper = mapper;
            _groupService = groupService;
            _columnService = columnService;
            _rowService = rowService;
            _cellService = cellService;
        }

        public void ImportSheet(string path, dynamic worksheetName, string groupName)
        {
            if (path == null)
                throw new InvalidParameterException("Path was not provided");

            if (worksheetName == null)
                throw new InvalidParameterException("WorksheetName was not provided");

            if (groupName == null)
                throw new InvalidParameterException("GroupName was not provided");

            var group = _groupService.GetGroup(groupName);

            using (XLWorkbook workBook = new XLWorkbook(path))
            {
                // first sheet only, need to implement for all sheets
                IXLWorksheet workSheet = workBook.Worksheets.FirstOrDefault();

                foreach (IXLColumn column in workSheet.Columns())
                {
                    if (!string.IsNullOrEmpty(column.ColumnLetter()))
                    {
                        _columnService.CreateColumn(new BusinessObjects.Column
                        {
                            Name = column.ColumnLetter(),
                            GroupId = group.Id
                        });
                    }
                }

                foreach (IXLRow row in workSheet.Rows())
                {
                    Entities.Row currentRow = null;

                    currentRow = _rowService.CreateRow(new Row
                    {
                        GroupId = group.Id
                    });

                    if (currentRow != null)
                    {
                        foreach (IXLCell cell in row.Cells())
                        {
                            if (!string.IsNullOrEmpty(cell.Value.ToString()))
                            {
                                _cellService.CreateCell(new BusinessObjects.Cell
                                {
                                    Data = cell.Value.ToString(),
                                    RowId = currentRow.Id
                                });
                            }
                        }
                    }

                }
            }
        }

        public (IList<Cell> records, int total, int totalDisplay) GetContacts(int pageIndex, int pageSize,
    string searchText, string sortText, string groupName)
        {

            var groupId = _groupService.GetGroup(groupName).Id;

            var rowsId = _importingUnitOfWork.Rows.GetDynamic(groupId == 0 ? null : x => x.GroupId == groupId,null,null,false)
                .Select(x => x.Id).ToList(); ;


            var cellsData = _importingUnitOfWork.Cells.GetDynamic(x => rowsId.Contains(x.RowId), sortText, string.Empty, pageIndex, pageSize);

            List<Cell> resultData;

            if (searchText != null)
            {
                resultData = (from cell in cellsData.data
                              where cell.Data.Contains(searchText)
                              select _mapper.Map<Cell>(cell)).ToList();
            }
            else
            {
                 resultData = (from cell in cellsData.data
                                  select _mapper.Map<Cell>(cell)).ToList();
            }



            return (resultData, cellsData.total, cellsData.totalDisplay);
        }
    }
}
