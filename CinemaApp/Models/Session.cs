namespace CinemaApp.Models
{
    public class Session
    {
        public int Id { get; set; }
        public int CinemaHallId { get; set; }
        public CinemaHall CinemaHall { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DurationInMinutes { get; set; }

    }
}
