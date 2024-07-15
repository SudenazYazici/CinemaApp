using CinemaApp.Models;

namespace CinemaApp.Dto
{
    public class SeatDto
    {
        public int Id { get; set; }
        public int SeatNum { get; set; }
        public int CinemaHallId { get; set; }
        //public CinemaHall CinemaHall { get; set; }
        //public Ticket Ticket { get; set; }
    }
}
