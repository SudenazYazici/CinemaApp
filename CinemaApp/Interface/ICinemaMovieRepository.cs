using CinemaApp.Models;

namespace CinemaApp.Interface
{
    public interface ICinemaMovieRepository
    {
        List<Cinema> GetCinemasOfMovie(int movieId);
        List<Movie> GetMoviesOfCinema(int cinemaId);

    }
}
