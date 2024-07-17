namespace CinemaApp.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public ICollection<CinemaMovie> CinemaMovies { get; set; } = [];
    }
}
