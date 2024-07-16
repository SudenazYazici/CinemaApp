using CinemaApp.Models;

namespace CinemaApp.Interface
{
    public interface ICinemaHallRepository
    {
        ICollection<CinemaHall> GetCinemaHalls();
        CinemaHall GetCinemaHall(int id);

        bool CinemaHallExists(int id);
        bool CreateCinemaHall(CinemaHall cinemaHall);
        bool UpdateCinemaHall(CinemaHall cinemaHall);
        bool DeleteCinemaHall(CinemaHall cinemaHall);
        bool Save();
        ICollection<Seat> GetSeats(int id);
    }
}
