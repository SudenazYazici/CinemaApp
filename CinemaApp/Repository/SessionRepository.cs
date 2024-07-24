using CinemaApp.Data;
using CinemaApp.Interface;
using CinemaApp.Models;

namespace CinemaApp.Repository
{
    public class SessionRepository : ISessionRepository
    {
        private readonly DataContext _context;

        public SessionRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateSession(Session session)
        {
            _context.Add(session);
            return Save();
        }

        public bool DeleteSession(Session session)
        {
            _context.Remove(session);
            return Save();
        }

        public Session GetSession(int id)
        {
            return _context.Sessions.Where(p => p.Id == id).FirstOrDefault();
        }

        public ICollection<Session> GetSessions(int movieId, int cinemaHallId)
        {
            return _context.Sessions
                .Where(p => p.MovieId == movieId && p.CinemaHallId == cinemaHallId).ToList();
        }

        public Session GetSession(int movieId, int cinemaHallId, DateTime startTime)
        {
            return _context.Sessions
                .Where(p => p.MovieId == movieId && p.CinemaHallId == cinemaHallId && p.StartDate == startTime).FirstOrDefault();
        }

        public ICollection<DateTime> GetSessionsStartTime(int movieId, int cinemaHallId)
        {
            var times = _context.Sessions.Where(p => p.MovieId == movieId && p.CinemaHallId == cinemaHallId)
                .Select(s => s.StartDate)
                .Distinct()
                .ToList();

            return times;
        }

        public ICollection<Session> GetSessions()
        {
            return _context.Sessions.OrderBy(p => p.Id).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool SessionExists(int id)
        {
            return _context.Sessions.Any(p => p.Id == id);
        }

        public bool UpdateSession(Session session)
        {
            _context.Update(session);
            return Save();
        }

        public ICollection<Session> GetSessionsOfMovie(int movieId)
        {
            var sessions = _context.Sessions
                .Where(s => s.MovieId == movieId)
                .ToList();

            return sessions;
        }

        public ICollection<CinemaHall> GetCinemaHallsOfMovie(int movieId)
        {
            var cinemaHalls = _context.Sessions
                .Where(s => s.MovieId == movieId)
                .Select(s => s.CinemaHall)
                .Distinct()
                .ToList();

            return cinemaHalls;
        }
    }
}
