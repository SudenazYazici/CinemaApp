using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Models
{
    public class Cinema
    {
        public int Id { get; set; }
        [MaxLength(80)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string City { get; set; }
        [MaxLength(150)]
        public string Address { get; set; }
        public ICollection<CinemaHall> CinemaHalls { get; set; } = new List<CinemaHall>();
        //public ICollection<Ticket> Tickets { get; set; }
        public ICollection<CinemaMovie> CinemaMovies { get; set; } = [];
        public ICollection<Session> Sessions { get; set; } = [];

    }
}
