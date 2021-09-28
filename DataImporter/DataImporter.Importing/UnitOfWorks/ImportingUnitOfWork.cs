using DataImporter.Data;
using DataImporter.Importing.Contexts;
using DataImporter.Importing.Repositories;
using Microsoft.EntityFrameworkCore;


namespace DataImporter.Importing.UnitOfWorks
{
    public class ImportingUnitOfWork : UnitOfWork, IImportingUnitOfWork
    {
        public IGroupRepository Groups { get; private set; }
        public IColumnRepository Columns { get; private set; }
        public IRowRepository Rows { get; private set; }
        public ICellRepository Cells { get; private set; }
        public IHistoryRepository Histories { get; private set; }

        public ImportingUnitOfWork(IImportingContext context,
            IGroupRepository groups, IColumnRepository columns, IRowRepository rows,
            ICellRepository cells, IHistoryRepository histories
            ) : base((DbContext)context)
        {
            Groups = groups;
            Columns = columns;
            Rows = rows;
            Cells = cells;
            Histories = histories;
        }
    }
}
