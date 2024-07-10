namespace CinemaApp.Models
{
    public class Cinema
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public ICollection<CinemaHall> CinemaHalls { get; set; }
        //public ICollection<Ticket> Tickets { get; set; }
        public ICollection<CinemaMovie> CinemaMovies { get; set; }

    }
}
