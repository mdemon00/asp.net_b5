using System;

namespace LibraryManagementSystem.Data
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();
    }
}
