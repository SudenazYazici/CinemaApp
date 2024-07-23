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
    }
}
