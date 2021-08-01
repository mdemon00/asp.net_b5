using Autofac;
using System;
using System.ComponentModel.DataAnnotations;
using TicketBookingSystem.Booking.BuisnessObjects;
using TicketBookingSystem.Booking.Services;

namespace TicketBookingSystem.Web.Areas.Admin.Models
{
    public class EditTicketModel
    {
        [Required, Range(1, 50000000)]
        public int? Id { get; set; }
        [Required, Range(1, 100000)]
        public int? CustomerId { get; set; }
        [Required, MaxLength(50, ErrorMessage = "Destination should be less than 50 charcaters")]
        public string Destination { get; set; }
        [Required, Range(1, 100000)]
        public double? TicketFee { get; set; }

        private readonly ITicketService _ticketService;
        public EditTicketModel()
        {
            _ticketService = Startup.AutofacContainer.Resolve<ITicketService>();
        }

        public EditTicketModel(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public void LoadModelData(int id)
        {
            var ticket = _ticketService.GetTicket(id);
            CustomerId = ticket?.CustomerId;
            Destination = ticket.Destination;
            TicketFee = ticket?.TicketFee;
        }

        internal void Update()
        {
            var ticket = new Ticket
            {
                Id = Id.HasValue ? Id.Value : 0,
                CustomerId = CustomerId.HasValue ? CustomerId.Value : 0,
                Destination = Destination,
                TicketFee = TicketFee.HasValue ? TicketFee.Value : 0,
            };
            _ticketService.UpdateTicket(ticket);
        }
    }
}
