using CinemaApp.Data;
using CinemaApp.Interface;
using CinemaApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Repository
{
    public class CinemaMovieRepository : ICinemaMovieRepository
    {
        private readonly DataContext _context;
        public CinemaMovieRepository(DataContext context) { _context = context; }

        public List<Cinema> GetCinemasOfMovie(int movieId)
        {
            var cinemas = _context.CinemaMovies
                .Where(cm => cm.movieId == movieId)
                .Include(cm => cm.cinema)
                .Select(cm => cm.cinema)
                .ToList();

            return cinemas;
        }

        public List<Movie> GetMoviesOfCinema(int cinemaId)
        {
            var movies = _context.CinemaMovies
                .Where(cm => cm.cinemaId == cinemaId)
                .Include(cm => cm.movie)
                .Select(c => c.movie)
                .ToList();

            return movies;
        }
    }
}
