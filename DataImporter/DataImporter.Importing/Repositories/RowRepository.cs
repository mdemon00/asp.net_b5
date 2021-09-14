using DataImporter.Data;
using DataImporter.Importing.Contexts;
using DataImporter.Importing.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataImporter.Importing.Repositories
{
    public class RowRepository : Repository<Row, int>,
IRowRepository
    {
        public RowRepository(IImportingContext context)
            : base((DbContext)context)
        {
        }
    }
}
