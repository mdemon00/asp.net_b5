using AutoMapper;
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

        public (IList<string[]> records, int total, int totalDisplay) GetContacts(int pageIndex, int pageSize,
    string searchText, string sortText, string groupName)
        {

            var groupId = _groupService.GetGroup(groupName).Id;
            var rowsId = _importingUnitOfWork.Rows.GetDynamic(groupId == 0 ? null : x => x.GroupId == groupId, null, null, false)
                .Skip(1)
                .Select(x => x.Id).ToList(); ;


            var cellsData = _importingUnitOfWork.Cells.GetDynamic(x => rowsId.Contains(x.RowId), null, null, false);
            var cellsGroupList = cellsData.GroupBy(x => x.RowId).ToList();

            var resultData = new List<string[]>();
            var total = cellsData.Count();
            var totalDisplay = cellsData.Count();

            foreach (var cellGroup in cellsGroupList)
            {
                var filteredCellGroup = new List<string>();
                var exist = false;

                if (searchText != null)
                {
                    foreach (var cell in cellGroup)
                    {
                        if (cell.Data.Contains(searchText))
                        {
                            exist = true;
                        }
                    }
                }

                if (exist || searchText == null)
                {
                    foreach (var cell in cellGroup)
                    {
                        filteredCellGroup.Add(cell.Data);
                    }

                    resultData.Add(filteredCellGroup.ToArray());

                }
            }

            totalDisplay = resultData.Count();

            if (sortText != null)
            {
                var pos = cellsData.First().Data == sortText.Split(" ")[0] ? 0 : 1;

                if (sortText.Split(" ")[1] == "asc")
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
