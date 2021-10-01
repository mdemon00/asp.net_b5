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
    public class ExcelService : IExcelService
    {
        private readonly IImportingUnitOfWork _importingUnitOfWork;
        private readonly IMapper _mapper;
        private readonly IGroupService _groupService;
        private readonly IColumnService _columnService;
        private readonly IRowService _rowService;
        private readonly ICellService _cellService;
        private readonly ILogger<ExcelService> _logger;

        public ExcelService(IImportingUnitOfWork importingUnitOfWork,
            IMapper mapper, IGroupService groupService, IColumnService columnService,
            IRowService rowService, ICellService cellService, ILogger<ExcelService> logger)
        {
            _importingUnitOfWork = importingUnitOfWork;
            _mapper = mapper;
            _groupService = groupService;
            _columnService = columnService;
            _rowService = rowService;
            _cellService = cellService;
            _logger = logger;
        }

        public DataTable ImportExceltoDatatable(string filePath, string sheetName)
        {
            using (XLWorkbook workBook = new XLWorkbook(filePath))
            {
                IXLWorksheet workSheet = workBook.Worksheet(1);

                DataTable dt = new DataTable();

                bool firstRow = true;
                var count = 1;
                foreach (IXLRow row in workSheet.Rows())
                {
                    if (count == 10)
                        return dt;
                    if (firstRow)
                    {
                        foreach (IXLCell cell in row.Cells())
                        {
                            dt.Columns.Add(cell.Value.ToString());
                        }
                        firstRow = false;
                    }
                    else
                    {
                        dt.Rows.Add();
                        int i = 0;

                        foreach (IXLCell cell in row.Cells(row.FirstCellUsed().Address.ColumnNumber, row.LastCellUsed().Address.ColumnNumber))
                        {
                            dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                            i++;
                        }
                    }
                    count++;
                }

                return dt;
            }
        }
        public void ImportSheet(string path, dynamic worksheetName, int groupId)
        {
            if (path == null)
                throw new InvalidParameterException("Path was not provided");

            if (worksheetName == null)
                throw new InvalidParameterException("WorksheetName was not provided");

            if (groupId < 1)
                throw new InvalidParameterException("GroupId was not provided");

            var group = _groupService.GetGroup(groupId, true);

            using (XLWorkbook workBook = new XLWorkbook(Path.Combine(path, worksheetName)))
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

        public void ExportSheet(string path, int groupId = 0)
        {
            if (groupId < 1)
                throw new InvalidParameterException("No group found");

            var group = _groupService.GetGroup(groupId,true);

            if (group == null)
                throw new InvalidParameterException("No group found");

            IList<string[]> records = new List<string[]> { };

            records = GetSheets(1, 1, null, null, groupId, true).records;

            if (records.Count() < 1)
                throw new InvalidParameterException("No records found");

            DataTable dt = new DataTable();

            dt.TableName = group.Name;

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

            using (XLWorkbook wb = new XLWorkbook())
            {
                //Add DataTable in worksheet  
                wb.Worksheets.Add(dt);

                try
                {
                    using (var fileStream = new FileStream(Path.Combine(path, group.Name + ".xlsx"), FileMode.Create))
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
        public (IList<string[]> records, int total, int totalDisplay) GetSheets(int pageIndex, int pageSize,
    string searchText, string sortText, int groupId = 0, bool export = false)
        {
            Group group = null;

            if (groupId < 1)
            {
                if (_groupService.GetAllGroups().Count > 1)
                    group = _groupService.GetAllGroups().FirstOrDefault();
                else
                    return (new List<string[]>() { }, 0, 0);
            }

            var rowsId = _rowService.GetAllRowsId(group == null ? groupId : group.Id);

            if (rowsId.Count < 1)
                return (new List<string[]>() { }, 0, 0);

            var cellsData = _cellService.GetCells(rowsId);

            if (cellsData.Count < 1)
                return (new List<string[]>() { }, 0, 0);

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

            if (export)
                return (resultData, total, totalDisplay);

            if (!string.IsNullOrEmpty(sortText))
            {
                var pos = 0;

                var columns = _columnService.GetAllColumns(group == null ? groupId : group.Id);

                if (columns.Count < 0)
                    pos = columns
                       .Select((Value, Index) => new { Value, Index })
                       .Where(p => p.Value.Name.Trim() == sortText.Split(" ")[0].Trim()).FirstOrDefault().Index;

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
