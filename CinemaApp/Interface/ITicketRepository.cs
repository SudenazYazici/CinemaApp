using CinemaApp.Models;

namespace CinemaApp.Interface
{
    public interface ITicketRepository
    {
        ICollection<Ticket> GetTickets();
        Ticket GetTicket(int id);

        bool TicketExists(int id);
        bool CreateTicket(Ticket ticket);
        bool UpdateTicket(Ticket ticket);
        bool DeleteTicket(Ticket ticket);
        bool Save();
    }
}
