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
    public class CellService : ICellService
    {
        private readonly IImportingUnitOfWork _importingUnitOfWork;
        private readonly IMapper _mapper;

        public CellService(IImportingUnitOfWork importingUnitOfWork,
            IMapper mapper)
        {
            _importingUnitOfWork = importingUnitOfWork;
            _mapper = mapper;
        }

        public IList<Cell> GetAllCells()
        {
            var cellEntities = _importingUnitOfWork.Cells.GetAll();
            var cells = new List<Cell>();

            foreach (var entity in cellEntities)
            {
                var cell = _mapper.Map<Cell>(entity);
                cells.Add(cell);
            }

            return cells;
        }

        public List<Cell> GetCells(IList<int> rowsId)
        {
            var cellEntities = _importingUnitOfWork.Cells.GetDynamic(x => rowsId.Contains(x.RowId), null, null, false);
            var cells = new List<Cell>();

            foreach (var entity in cellEntities)
            {
                var cell = _mapper.Map<Cell>(entity);
                cells.Add(cell);
            }

            return cells;
        }

        public void CreateCell(Cell cell)
        {
            if (cell == null)
                throw new InvalidParameterException("Cell was not provided");

            _importingUnitOfWork.Cells.Add(
                _mapper.Map<Entities.Cell>(cell)
            );

            _importingUnitOfWork.Save();
        }


        public Cell GetCell(int id)
        {
            var cell = _importingUnitOfWork.Cells.GetById(id);

            if (cell == null) return null;

            return _mapper.Map<Cell>(cell);
        }

        public Cell GetCell(string rowId)
        {

            int value = 0;
            int.TryParse(rowId, out value); // determine whether a string represents a numeric value

            var cell = _importingUnitOfWork.Cells.GetDynamic(value == 0 ? null : x => x.RowId.ToString().Contains(value.ToString()));

            if (cell == null) return null;

            return _mapper.Map<Cell>(cell);

        }
        public void UpdateCell(Cell cell)
        {
            if (cell == null)
                throw new InvalidOperationException("Cell is missing");

            var cellEntity = _importingUnitOfWork.Cells.GetById(cell.Id);

            if (cellEntity != null)
            {
                _mapper.Map(cell, cellEntity);
                _importingUnitOfWork.Save();
            }
            else
                throw new InvalidOperationException("Couldn't find cell");
        }

        public void DeleteCell(int id)
        {
            _importingUnitOfWork.Cells.Remove(id);
            _importingUnitOfWork.Save();
        }


    }
}
