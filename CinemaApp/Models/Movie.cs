using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Details { get; set; }
        public ICollection<CinemaMovie> CinemaMovies { get; set; } = [];
        public ICollection<Session> Sessions { get; set; } = [];
    }
}
