using CinemaApp.Data;
using CinemaApp.Interface;
using CinemaApp.Models;

namespace CinemaApp.Repository
{
    public class SeatRepository : ISeatRepository
    {
        private readonly DataContext _context;
        public SeatRepository(DataContext context) { _context = context; }
        public Seat GetSeat(int id)
        {
            return _context.Seats.Where(p => p.Id == id).FirstOrDefault();
        }

        public ICollection<Seat> GetSeats()
        {
            return _context.Seats.OrderBy(p => p.Id).ToList();
        }

        public bool SeatExists(int id)
        {
            return _context.Seats.Any(p => p.Id == id);
        }

        public bool CreateSeat(Seat seat)
        {
            _context.Add(seat);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateSeat(Seat seat)
        {
            _context.Update(seat);
            return Save();
        }

        public bool DeleteSeat(Seat seat) { 
            _context.Remove(seat);
            return Save();
        }
    }
}
