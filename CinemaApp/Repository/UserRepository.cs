using CinemaApp.Data;
using CinemaApp.Interface;
using CinemaApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context) { _context = context; }
        public User GetUser(int id)
        {
            return _context.Users.Where(p => p.Id == id).FirstOrDefault();
        }

        public User GetUser(string name)
        {
            return _context.Users.Where(p => p.Name == name).FirstOrDefault();
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.Where(p => p.Email == email).FirstOrDefault();
        }

        public ICollection<User> GetUsers()
        {
            return _context.Users.OrderBy(p => p.Id).ToList();
        }

        public bool UserExists(int id)
        {
            return _context.Users.Any(p => p.Id == id);
        }
        public bool UserExists(string name)
        {
            return _context.Users.Any(p => p.Name == name);
        }
        public ICollection<Ticket> GetTicketsByUser(int id)
        {
            return _context.Users
            .Where(u => u.Id == id)
            .Include(u => u.Tickets)
            .SelectMany(u => u.Tickets)
            .ToList();
        }

        public bool CreateUser(User user){
            _context.Add(user);
            return Save();
        }

        public bool Save() { 
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateUser(User user)
        {
            _context.Update(user);
            return Save();
        }

        public bool DeleteUser(User user) { 
            _context.Remove(user);
            return Save();
        }

        /*public User logIn(string email, string password)
        {
            return _context.Users.Where(p => p.Email == email && p.Password == password).FirstOrDefault();
        }*/
    }
}
