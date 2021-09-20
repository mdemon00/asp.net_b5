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
    public class RowService : IRowService
    {
        private readonly IImportingUnitOfWork _importingUnitOfWork;
        private readonly IMapper _mapper;

        public RowService(IImportingUnitOfWork importingUnitOfWork,
            IMapper mapper)
        {
            _importingUnitOfWork = importingUnitOfWork;
            _mapper = mapper;
        }

        public IList<Row> GetAllRows()
        {
            var rowEntities = _importingUnitOfWork.Rows.GetAll();
            var rows = new List<Row>();

            foreach (var entity in rowEntities)
            {
                var row = _mapper.Map<Row>(entity);
                rows.Add(row);
            }

            return rows;
        }

        public IList<int> GetAllRowsId(int groupId)
        {
            var rowsId = _importingUnitOfWork.Rows.GetDynamic(groupId == 0 ? null : x => x.GroupId == groupId, null, null, false)
                .Skip(1)
                .Select(x => x.Id).ToList();

            return rowsId;
        }

        public Entities.Row CreateRow(Row row)
        {
            if (row == null)
                throw new InvalidParameterException("Row was not provided");

            var rowEntity = _mapper.Map<Entities.Row>(row);

            _importingUnitOfWork.Rows.Add(rowEntity);

            _importingUnitOfWork.Save();

            return rowEntity;
        }

        public Row GetRow(int id)
        {
            var row = _importingUnitOfWork.Rows.GetById(id);

            if (row == null) return null;

            return _mapper.Map<Row>(row);
        }

        public Row GetRow(string groupId)
        {

            int value = 0;
            int.TryParse(groupId, out value); // determine whether a string represents a numeric value

            var row = _importingUnitOfWork.Rows.GetDynamic(value == 0 ? null : x => x.GroupId.ToString().Contains(value.ToString()));

            if (row == null) return null;

            return _mapper.Map<Row>(row);

        }
        public void UpdateRow(Row row)
        {
            if (row == null)
                throw new InvalidOperationException("Row is missing");

            //if (IsNameAlreadyUsed(row.Name, row.Id))
            //    throw new DuplicateNameException("Row name already used in other row.");

            var rowEntity = _importingUnitOfWork.Rows.GetById(row.Id);

            if (rowEntity != null)
            {
                _mapper.Map(row, rowEntity);
                _importingUnitOfWork.Save();
            }
            else
                throw new InvalidOperationException("Couldn't find row");
        }

        public void DeleteRow(int id)
        {
            _importingUnitOfWork.Rows.Remove(id);
            _importingUnitOfWork.Save();
        }
    }
}
