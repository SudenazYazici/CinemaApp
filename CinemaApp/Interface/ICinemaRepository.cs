using CinemaApp.Models;

namespace CinemaApp.Interface
{
    public interface ICinemaRepository
    {
        ICollection<Cinema> GetCinemas();
        Cinema GetCinema(int id);

        bool CinemaExists(int id);
        bool CreateCinema(Cinema cinema);
        bool UpdateCinema(Cinema cinema);
        bool DeleteCinema(Cinema cinema);
        bool Save();
        ICollection<CinemaHall> GetCinemaHalls(int id);
        //ICollection<CinemaMovie> GetMovies(int id);
    }
}
