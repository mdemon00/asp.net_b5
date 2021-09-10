using DataImporter.Data;
using DataImporter.Importing.Contexts;
using DataImporter.Importing.Repositories;
using Microsoft.EntityFrameworkCore;


namespace DataImporter.Importing.UnitOfWorks
{
    public class ImportingUnitOfWork : UnitOfWork, IImportingUnitOfWork
    {
        public IGroupRepository Groups { get; private set; }

        public ImportingUnitOfWork(IImportingContext context,
            IGroupRepository groups
            ) : base((DbContext)context)
        {
            Groups = groups;
        }
    }
}
