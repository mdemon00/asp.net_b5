namespace TicketBookingSystem.Booking.BuisnessObjects
{
    public class Ticket
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Destination { get; set; }
        public double TicketFee { get; set; }
    }
}
