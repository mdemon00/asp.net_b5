using Autofac;
using System;
using System.ComponentModel.DataAnnotations;
using TicketBookingSystem.Booking.BuisnessObjects;
using TicketBookingSystem.Booking.Services;

namespace TicketBookingSystem.Web.Areas.Admin.Models
{
    public class CreateTicketModel
    {
        [Required, Range(1, 100000)]
        public int CustomerId { get; set; }
        [Required, MaxLength(50, ErrorMessage = "Destination should be less than 50 charcaters")]
        public string Destination { get; set; }
        [Required, Range(1, 100000)]
        public double TicketFee { get; set; }

        private readonly ITicketService _ticketService;
        public CreateTicketModel()
        {
            _ticketService = Startup.AutofacContainer.Resolve<ITicketService>();
        }

        public CreateTicketModel(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        internal void CreateTicket()
        {
            var ticket = new Ticket
            {
                CustomerId = CustomerId,
                Destination = Destination,
                TicketFee = TicketFee
            };

            _ticketService.CreateTicket(ticket);
        }
    }
}
