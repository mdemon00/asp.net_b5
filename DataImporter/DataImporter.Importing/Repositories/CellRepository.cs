using DataImporter.Data;
using DataImporter.Importing.Contexts;
using DataImporter.Importing.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataImporter.Importing.Repositories
{
    public class CellRepository : Repository<Cell, int>,
ICellRepository
    {
        public CellRepository(IImportingContext context)
            : base((DbContext)context)
        {
        }
    }
}
