using CinemaApp.Models;

namespace CinemaApp.Interface
{
    public interface ISeatRepository
    {
        ICollection<Seat> GetSeats();
        Seat GetSeat(int id);

        bool SeatExists(int id);
        bool CreateSeat(Seat seat);
        bool UpdateSeat(Seat seat);
        bool DeleteSeat(Seat seat);
        bool Save();
    }
}
