using DataImporter.Data;
using DataImporter.Importing.Contexts;
using DataImporter.Importing.Entities;
using DataImporter.Importing.Repositories;

namespace DataImporter.Importing.UnitOfWorks
{
    public interface IImportingUnitOfWork : IUnitOfWork
    {
        IGroupRepository Groups { get; }
    }
}