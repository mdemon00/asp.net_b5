using AutoMapper;
using ClosedXML.Excel;
using DataImporter.Importing.BusinessObjects;
using DataImporter.Importing.Exceptions;
using DataImporter.Importing.UnitOfWorks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
        private readonly ILogger<ContactService> _logger;
        private IWebHostEnvironment _environment;

        public ContactService(IImportingUnitOfWork importingUnitOfWork,
            IWebHostEnvironment environment,
            IMapper mapper, IGroupService groupService, IColumnService columnService,
            IRowService rowService, ICellService cellService, ILogger<ContactService> logger)
        {
            _importingUnitOfWork = importingUnitOfWork;
            _mapper = mapper;
            _groupService = groupService;
            _columnService = columnService;
            _rowService = rowService;
            _cellService = cellService;
            _logger = logger;
            _environment = environment;
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

                var count = 0;

                foreach (IXLRow row in workSheet.Rows())
                {
                    Entities.Row currentRow = null;

                    currentRow = _rowService.CreateRow(new Row
                    {
                        GroupId = group.Id
                    });

                    if (currentRow != null)
                    {
                        count++;
                        foreach (IXLCell cell in row.Cells())
                        {
                            if (!string.IsNullOrEmpty(cell.Value.ToString()))
                            {
                                try
                                {
                                    _cellService.CreateCell(new BusinessObjects.Cell
                                    {
                                        Data = cell.Value.ToString(),
                                        RowId = currentRow.Id
                                    });

                                    if (count == 1) //Inserting first row as column
                                        _columnService.CreateColumn(new BusinessObjects.Column
                                        {
                                            Name = cell.Value.ToString(),
                                            GroupId = group.Id
                                        });
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError(ex.ToString());
                                    throw new InvalidOperationException("Import failed");

                                }

                            }
                        }
                    }

                }
            }
        }

        public void ExportSheet(List<String> groupNames)
        {
            if(groupNames.Count < 1)
                throw new InvalidParameterException("No groups found");

            IList<string[]> records = new List<string[]> { };

            try
            {
                 records = GetContacts(1, 1, null, null, groupNames[0], true).records;
            }
            catch(Exception ex)
            {
                throw new InvalidParameterException("Something Went Wrong " + ex);
            }

            if(records == null || records.Count() < 1)
                throw new InvalidParameterException("Something Went Wrong ");

            DataTable dt = new DataTable();

            dt.TableName = groupNames[0];

            var count = 0;
            foreach (var row in records)
            {
                if (count == 0)
                {
                    foreach (var cell in row)
                    {
                        dt.Columns.Add(cell);
                    }
                }
                else
                {
                    dt.Rows.Add();
                    int cellIndex = 0;
                    foreach (var cell in row)
                    {
                        dt.Rows[dt.Rows.Count - 1][cellIndex] = cell;
                        cellIndex++;
                    }
                }
                count++;
            }

            //Name of File  
            string fileName = groupNames[0] + ".xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                //Add DataTable in worksheet  
                wb.Worksheets.Add(dt);

                var downloads = Path.Combine(_environment.WebRootPath, "downloads");

                try
                {
                    using (var fileStream = new FileStream(Path.Combine(downloads, fileName), FileMode.Create))
                    {
                        wb.SaveAs(fileStream);
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidParameterException("Export failed " + ex);
                }
            }

        }
        public (IList<string[]> records, int total, int totalDisplay) GetContacts(int pageIndex, int pageSize,
    string searchText, string sortText, string groupName, bool export = false)
        {
            int groupId;

            if (string.IsNullOrEmpty(groupName))
            {
                groupId = _groupService.GetAllGroups().FirstOrDefault().Id;
            }
            else
            {
                groupId = _groupService.GetGroup(groupName).Id;
            }

            var rowsId = _rowService.GetAllRowsId(groupId);

            if (rowsId.Count < 1)
                return (new List<string[]>() { }, 0, 0);

            var cellsData = _cellService.GetCells(rowsId);

            var cellsGroupList = cellsData.GroupBy(x => x.RowId).ToList();

            var resultData = new List<string[]>();
            var total = cellsData.Count();
            var totalDisplay = cellsData.Count();

            foreach (var cellGroup in cellsGroupList)
            {
                var filteredCellGroup = new List<string>();
                var exist = false;

                if (!string.IsNullOrEmpty(searchText))
                {
                    foreach (var cell in cellGroup)
                    {
                        if (cell.Data.Contains(searchText))
                        {
                            exist = true;
                        }
                    }
                }

                if (exist || string.IsNullOrEmpty(searchText))
                {
                    foreach (var cell in cellGroup)
                    {
                        filteredCellGroup.Add(cell.Data);
                    }

                    resultData.Add(filteredCellGroup.ToArray());

                }
            }

            totalDisplay = resultData.Count();

            if(export)
                return (resultData, total, totalDisplay);

            if (!string.IsNullOrEmpty(sortText))
            {
                var pos = 0;

                try
                {
                    pos = _columnService.GetAllColumns(groupName)
                       .Select((Value, Index) => new { Value, Index })
                       .Where(p => p.Value.Name.Trim() == sortText.Split(" ")[0].Trim()).FirstOrDefault().Index;
                }
                catch (Exception ex)
                {
                    _logger.LogError("Getting position failed" + ex);
                }

                if (sortText.Split(" ").Contains("asc"))
                {
                    resultData = resultData.OrderBy(o => o[pos])
                        .Skip((pageIndex - 1) * pageSize).Take(pageSize)
                        .ToArray().ToList();
                }
                else
                {
                    resultData = resultData.OrderByDescending(o => o[pos])
                        .Skip((pageIndex - 1) * pageSize).Take(pageSize)
                        .ToArray().ToList();
                }
                return (resultData, total, totalDisplay);
            }
            else
            {
                resultData = resultData.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                return (resultData, total, totalDisplay);
            }
        }

        public List<Column> GetColums()
        {
            List<Column> _columns = new List<Column>();

            var columns = _columnService.GetAllColumns();

            _mapper.Map(columns, _columns);

            return _columns;
        }
    }
}
