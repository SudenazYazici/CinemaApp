namespace CinemaApp.Models
{
    public class CinemaHall
    {
        public int Id { get; set; }
        public int HallNum { get; set; }
        public int? CinemaId { get; set; }
        public Cinema Cinema { get; set; }
        public ICollection<Seat> Seats { get; set; } = new List<Seat>();
        //public ICollection<Ticket> Tickets { get; set; }
        public ICollection<Session> Sessions { get; set; } = [];
    }
}
