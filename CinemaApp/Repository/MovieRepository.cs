using CinemaApp.Data;
using CinemaApp.Interface;
using CinemaApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly DataContext _context;
        public MovieRepository(DataContext context) { _context = context; }
        public Movie GetMovie(int id)
        {
            return _context.Movies.Where(p => p.Id == id).FirstOrDefault();
        }

        public Movie GetMovie(string name)
        {
            return _context.Movies.Where(p => p.Name == name).FirstOrDefault();
        }

        public ICollection<Movie> GetMovies()
        {
            return _context.Movies.OrderBy(p => p.Id).ToList();
        }

        public bool MovieExists(int id)
        {
            return _context.Movies.Any(p => p.Id == id);
        }

        public bool CreateMovie(Movie movie)
        {
            _context.Add(movie);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateMovie(Movie movie)
        {
            _context.Update(movie);
            return Save();
        }

        public bool DeleteMovie(Movie movie)
        {
            _context.Remove(movie);
            return Save();
        }
    }
}
