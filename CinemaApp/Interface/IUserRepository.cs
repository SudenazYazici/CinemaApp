using CinemaApp.Models;

namespace CinemaApp.Interface
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
        User GetUser(int id);
        User GetUser(string name);
        
        bool UserExists(int id);
        ICollection<Ticket> GetTicketsByUser(int id);
        bool CreateUser(User user);
        bool UpdateUser(User user);
        bool DeleteUser(User user);
        bool Save();
        //User logIn(string email, string password);
    }
}
