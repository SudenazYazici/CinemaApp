using CinemaApp.Data;
using CinemaApp.Interface;
using CinemaApp.Models;

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
    }
}
