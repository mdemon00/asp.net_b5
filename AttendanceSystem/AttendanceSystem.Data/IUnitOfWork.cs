using System;

namespace AttendanceSystem.Data
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();
    }
}
