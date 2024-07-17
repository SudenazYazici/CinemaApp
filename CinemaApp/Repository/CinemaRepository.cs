using CinemaApp.Data;
using CinemaApp.Interface;
using CinemaApp.Models;

namespace CinemaApp.Repository
{
    public class CinemaRepository : ICinemaRepository
    {
        private readonly DataContext _context;
        public CinemaRepository(DataContext context) { _context = context; }
        public Cinema GetCinema(int id)
        {
            return _context.Cinemas.Where(p => p.Id == id).FirstOrDefault();
        }

        public Cinema GetCinema(string name)
        {
            return _context.Cinemas.Where(p => p.Name == name).FirstOrDefault();
        }

        public ICollection<Cinema> GetCinemas()
        {
            return _context.Cinemas.OrderBy(p => p.Id).ToList();
        }

        public bool CinemaExists(int id)
        {
            return _context.Cinemas.Any(p => p.Id == id);
        }

        public bool CreateCinema(Cinema cinema)
        {
            _context.Add(cinema);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCinema(Cinema cinema)
        {
            _context.Update(cinema);
            return Save();
        }

        public bool DeleteCinema(Cinema cinema)
        {
            _context.Remove(cinema);
            return Save();
        }

        public ICollection<CinemaHall> GetCinemaHalls(int id)
        {
            return _context.CinemaHalls.Where(ch => ch.CinemaId == id).OrderBy(ch => ch.Id).ToList();
        }

        //public ICollection<CinemaMovie> GetMovies(int id) // check later
        //{
        //    return _context.CinemaMovies.Where(ch => ch.cinemaId == id).OrderBy(ch => ch.movieId).ToList();
        //}
    }
}
