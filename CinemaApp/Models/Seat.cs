namespace CinemaApp.Models
{
    public class Seat
    {
        public int Id { get; set; }
        public int SeatNum { get; set; }
        public int? CinemaHallId { get; set; }
        public CinemaHall? CinemaHall { get; set; }
        public Ticket? Ticket { get; set; }
    }
}
