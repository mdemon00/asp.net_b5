using AutoMapper;
using DataImporter.Importing.BusinessObjects;
using DataImporter.Importing.Exceptions;
using DataImporter.Importing.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DataImporter.Importing.Services
{
    public class ColumnService : IColumnService
    {
        private readonly IImportingUnitOfWork _importingUnitOfWork;
        private readonly IMapper _mapper;

        public ColumnService(IImportingUnitOfWork importingUnitOfWork,
            IMapper mapper)
        {
            _importingUnitOfWork = importingUnitOfWork;
            _mapper = mapper;
        }

        public IList<Column> GetAllColumns()
        {
            var columnEntities = _importingUnitOfWork.Columns.GetAll();
            var columns = new List<Column>();

            foreach (var entity in columnEntities)
            {
                var column = _mapper.Map<Column>(entity);
                columns.Add(column);
            }

            return columns;
        }

        public void CreateColumn(Column column)
        {
            if (column == null)
                throw new InvalidParameterException("Column was not provided");

            if (IsNameAlreadyUsed(column.Name))
                throw new DuplicateNameException("Column name already exists");

            _importingUnitOfWork.Columns.Add(
                _mapper.Map<Entities.Column>(column)
            );

            _importingUnitOfWork.Save();
        }

        private bool IsNameAlreadyUsed(string name) =>
            _importingUnitOfWork.Columns.GetCount(x => x.Name == name) > 0;

        private bool IsNameAlreadyUsed(string name, int id) =>
            _importingUnitOfWork.Columns.GetCount(x => x.Name == name && x.Id != id) > 0;

        public (IList<Column> records, int total, int totalDisplay) GetColumns(int pageIndex, int pageSize,
            string searchText, string sortText)
        {
            var columnData = _importingUnitOfWork.Columns.GetDynamic(
                string.IsNullOrWhiteSpace(searchText) ? null : x => x.Name.Contains(searchText),
                sortText, string.Empty, pageIndex, pageSize);

            var resultData = (from gr in columnData.data
                              select _mapper.Map<Column>(gr)).ToList();

            return (resultData, columnData.total, columnData.totalDisplay);
        }

        public Column GetColumn(int id)
        {
            var column = _importingUnitOfWork.Columns.GetById(id);

            if (column == null) return null;

            return _mapper.Map<Column>(column);
        }

        public Column GetColumn(string groupId)
        {

            int value = 0;
            int.TryParse(groupId, out value); // determine whether a string represents a numeric value

            var column = _importingUnitOfWork.Columns.GetDynamic(value == 0 ? null : x => x.GroupId.ToString().Contains(value.ToString()));

            if (column == null) return null;

            return _mapper.Map<Column>(column);

        }

        public void UpdateColumn(Column column)
        {
            if (column == null)
                throw new InvalidOperationException("Column is missing");

            if (IsNameAlreadyUsed(column.Name, column.Id))
                throw new DuplicateNameException("Column name already used in other column.");

            var columnEntity = _importingUnitOfWork.Columns.GetById(column.Id);

            if (columnEntity != null)
            {
                _mapper.Map(column, columnEntity);
                _importingUnitOfWork.Save();
            }
            else
                throw new InvalidOperationException("Couldn't find column");
        }

        public void DeleteColumn(int id)
        {
            _importingUnitOfWork.Columns.Remove(id);
            _importingUnitOfWork.Save();
        }
    }
}
