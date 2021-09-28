using DataImporter.Data;
using DataImporter.Importing.Contexts;
using DataImporter.Importing.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataImporter.Importing.Repositories
{
    public class HistoryRepository : Repository<History, int>, IHistoryRepository
    {
        public HistoryRepository(IImportingContext context)
            : base((DbContext)context)
        {
        }
    }
}
