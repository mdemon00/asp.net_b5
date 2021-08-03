using System;

namespace InventorySystem.Data
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();
    }
}
