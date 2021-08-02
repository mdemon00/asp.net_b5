using System;

namespace ECommerceSystem.Data
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();
    }
}
