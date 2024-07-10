namespace CinemaApp.Models
{
    public class CinemaMovie
    {
        public int cinemaId { get; set; }
        public int movieId { get; set; }
        public Cinema cinema { get; set; }
        public Movie movie { get; set; }

    }
}
