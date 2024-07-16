using CinemaApp.Data;
using CinemaApp.Interface;
using CinemaApp.Models;

namespace CinemaApp.Repository
{
    public class CinemaHallRepository : ICinemaHallRepository
    {
        private readonly DataContext _context;
        public CinemaHallRepository(DataContext context) { _context = context; }
        public CinemaHall GetCinemaHall(int id)
        {
            return _context.CinemaHalls.Where(p => p.Id == id).FirstOrDefault();
        }

        public ICollection<CinemaHall> GetCinemaHalls()
        {
            return _context.CinemaHalls.OrderBy(p => p.Id).ToList();
        }

        public bool CinemaHallExists(int id)
        {
            return _context.CinemaHalls.Any(p => p.Id == id);
        }

        public bool CreateCinemaHall(CinemaHall cinemaHall)
        {
            _context.Add(cinemaHall);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCinemaHall(CinemaHall cinemaHall)
        {
            _context.Update(cinemaHall);
            return Save();
        }

        public bool DeleteCinemaHall(CinemaHall cinemaHall)
        {
            _context.Remove(cinemaHall);
            return Save();
        }

        public ICollection<Seat> GetSeats(int id)
        {
            return _context.Seats.Where(ch => ch.CinemaHallId == id).OrderBy(ch => ch.Id).ToList();
        }
    }
}
