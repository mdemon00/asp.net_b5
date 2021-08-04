using System;

namespace SocialNetwork.Data
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();
    }
}
