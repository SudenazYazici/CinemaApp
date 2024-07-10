using CinemaApp.Models;

namespace CinemaApp.Dto
{
    public class CinemaHallDto
    {
        public int Id { get; set; }
        public int HallNum { get; set; }
        public int CinemaId { get; set; }
        public Cinema Cinema { get; set; }
    }
}
