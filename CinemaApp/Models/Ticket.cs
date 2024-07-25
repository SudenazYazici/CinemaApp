namespace CinemaApp.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string MovieName { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int CinemaId { get; set; }
        //public Cinema Cinema { get; set; }
        public int CinemaHallId { get; set; }
        //public CinemaHall CinemaHall { get; set; }
        public int SeatId { get; set; }
        public Seat Seat { get; set; }
        public int SessionId { get; set; }
        public Session Session { get; set; }
        public DateTime Date { get; set; }
        public int Price { get; set; }

    }
}
