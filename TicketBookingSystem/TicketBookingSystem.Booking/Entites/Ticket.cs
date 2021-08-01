using TicketBookingSystem.Data;

namespace TicketBookingSystem.Booking.Entites
{
    public class Ticket : IEntity<int>
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Destination { get; set; }
        public double TicketFee { get; set; }
    }
}
