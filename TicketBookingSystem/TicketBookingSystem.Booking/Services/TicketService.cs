using System;
using System.Collections.Generic;
using System.Linq;
using TicketBookingSystem.Booking.BuisnessObjects;
using TicketBookingSystem.Booking.Exceptions;
using TicketBookingSystem.Booking.UnitOfWorks;

namespace TicketBookingSystem.Booking.Services
{
    public class TicketService : ITicketService
    {
        private readonly IBookingUnitOfWork _bookingUnitOfWork;

        public TicketService(IBookingUnitOfWork bookingUnitOfWork)
        {
            _bookingUnitOfWork = bookingUnitOfWork;
        }

        public void CreateTicket(Ticket ticket)
        {
            if (ticket == null)
                throw new InvalidParameterException("Ticket was not provided");

            _bookingUnitOfWork.Tickets.Add(new Entites.Ticket
            {
                CustomerId = ticket.CustomerId,
                Destination = ticket.Destination,
                TicketFee = ticket.TicketFee
            });

            _bookingUnitOfWork.Save();
        }

        public void DeleteTicket(int id)
        {
            _bookingUnitOfWork.Tickets.Remove(id);
            _bookingUnitOfWork.Save();
        }

        public Ticket GetTicket(int id)
        {
            var ticket = _bookingUnitOfWork.Tickets.GetById(id);

            if (ticket == null) return null;

            return new Ticket
            {
                Id = ticket.Id,
                CustomerId = ticket.CustomerId,
                Destination = ticket.Destination,
                TicketFee = ticket.TicketFee
            };
        }

        public (IList<Ticket> records, int total, int totalDisplay) GetTickets(int pageIndex, int pageSize, string searchText, string sortText)
        {

            int value = 0;
            int.TryParse(searchText, out value); // determine whether a string represents a numeric value

            var ticketData = _bookingUnitOfWork.Tickets.GetDynamic(value == 0 ? null : x => x.CustomerId.ToString().Contains(value.ToString()),
            sortText, string.Empty, pageIndex, pageSize);

            var resultData = (from ticket in ticketData.data
                              select new Ticket
                              {
                                  Id = ticket.Id,
                                  CustomerId = ticket.CustomerId,
                                  Destination = ticket.Destination,
                                  TicketFee = ticket.TicketFee
                              }).ToList();
            return (resultData, ticketData.total, ticketData.totalDisplay);
        }

        public void UpdateTicket(Ticket ticket)
        {
            if (ticket == null)
                throw new InvalidOperationException("Ticket is missing");

            var ticketEntity = _bookingUnitOfWork.Tickets.GetById(ticket.Id);

            if (ticketEntity != null)
            {
                ticketEntity.CustomerId = ticket.CustomerId;
                ticketEntity.Destination = ticket.Destination;
                ticketEntity.TicketFee = ticket.TicketFee;

                _bookingUnitOfWork.Save();
            }
            else
                throw new InvalidOperationException("Couldn't find ticket");
        }
    }
}