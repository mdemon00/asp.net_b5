using DataImporter.Data;
using DataImporter.Importing.Contexts;
using DataImporter.Importing.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataImporter.Importing.Repositories
{
    public class ColumnRepository : Repository<Column, int>,
    IColumnRepository
    {
        public ColumnRepository(IImportingContext context)
            : base((DbContext)context)
        {
        }
    }
}
