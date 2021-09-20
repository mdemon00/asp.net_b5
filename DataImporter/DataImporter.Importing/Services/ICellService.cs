using DataImporter.Importing.BusinessObjects;
using System.Collections.Generic;


namespace DataImporter.Importing.Services
{
    public interface ICellService
    {
        List<Cell> GetCells(IList<int> rowsId);
        void CreateCell(Cell cell);
        Cell GetCell(int id);
        Cell GetCell(string name);
        void UpdateCell(Cell cell);
        void DeleteCell(int id);
    }
}
