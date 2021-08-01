using System;

namespace TicketBookingSystem.Data
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();
    }
}
