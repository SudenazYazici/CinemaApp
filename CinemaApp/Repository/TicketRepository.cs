using CinemaApp.Data;
using CinemaApp.Interface;
using CinemaApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Repository
{
    public class TicketRepository : ITicketRepository
    {
        private readonly DataContext _context;
        public TicketRepository(DataContext context) { _context = context; }
        public Ticket GetTicket(int id)
        {
            return _context.Tickets.Where(p => p.Id == id).FirstOrDefault();
        }

        public ICollection<Ticket> GetTickets()
        {
            return _context.Tickets.OrderBy(p => p.Id).ToList();
        }

        public bool TicketExists(int id)
        {
            return _context.Tickets.Any(p => p.Id == id);
        }

        public bool CreateTicket(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateTicket(Ticket ticket)
        {
            _context.Update(ticket);
            return Save();
        }

        public bool DeleteTicket(Ticket ticket) {
            _context.Remove(ticket);
            return Save();
        }

        public IEnumerable<int> GetUnavailableSeatIds(int cinemaId, int movieId, int cinemaHallId, DateTime startTime)
        {
            var tickets = _context.Tickets
                .Include(t => t.Session)
                .ToList();

            var unavailableSeats = tickets
                .Where(t => t.Session.CinemaId == cinemaId &&  t.Session.MovieId == movieId && t.Session.CinemaHallId == cinemaHallId && t.Session.StartDate == startTime)
                .Select(t => t.SeatId)
                .Distinct()
                .ToList();

            return unavailableSeats;
        }
    }
}
