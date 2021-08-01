using System.Collections.Generic;
using TicketBookingSystem.Booking.BuisnessObjects;

namespace TicketBookingSystem.Booking.Services
{
    public interface ITicketService
    {
        (IList<Ticket> records, int total, int totalDisplay) GetTickets(int pageIndex, int pageSize, string searchText, string sortText);
        void CreateTicket(Ticket ticket);
        Ticket GetTicket(int id);
        void UpdateTicket(Ticket ticket);
        void DeleteTicket(int id);
    }
}