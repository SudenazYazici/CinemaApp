using CinemaApp.Models;

namespace CinemaApp.Dto
{
    public class SessionDto
    {
        public int Id { get; set; }
        public int CinemaId { get; set; }
        public int CinemaHallId { get; set; }
        public int MovieId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DurationInMinutes { get; set; }
    }
}
