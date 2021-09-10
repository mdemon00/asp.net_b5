using DataImporter.Data;
using DataImporter.Importing.Contexts;
using DataImporter.Importing.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataImporter.Importing.Repositories
{
    public class GroupRepository : Repository<Group, int>,
        IGroupRepository
    {
        public GroupRepository(IImportingContext context)
            : base((DbContext)context)
        {
        }
    }
}
