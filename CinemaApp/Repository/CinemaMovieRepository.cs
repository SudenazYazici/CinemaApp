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

        public void AddCinemaMovie(int movieId, int cinemaId)
        {
            var cinema = _context.Cinemas.Include(c => c.CinemaMovies).FirstOrDefault(c => c.Id == cinemaId);
            var movie = _context.Movies.Include(c => c.CinemaMovies).FirstOrDefault(m => m.Id == movieId);
            var cinemaMovie = new CinemaMovie
            {
                cinemaId = cinemaId,
                cinema = cinema,
                movieId = movieId,
                movie = movie
            };
            cinema.CinemaMovies.Add(cinemaMovie);
            movie.CinemaMovies.Add(cinemaMovie);

            _context.SaveChanges();
        }

        public void RemoveCinemaMovie(int movieId, int cinemaId)
        {
            var cinema = _context.Cinemas.Include(c => c.CinemaMovies).FirstOrDefault(c => c.Id == cinemaId);
            var movie = _context.Movies.Include(c => c.CinemaMovies).FirstOrDefault(m => m.Id == movieId);
            var cinemaMovie = _context.CinemaMovies.FirstOrDefault(cm => cm.movieId == movieId && cm.cinemaId == cinemaId);
            cinema.CinemaMovies.Remove(cinemaMovie);
            movie.CinemaMovies.Remove(cinemaMovie);

            _context.SaveChanges();
        }
    }
}
