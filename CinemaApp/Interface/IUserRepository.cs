using CinemaApp.Models;

namespace CinemaApp.Interface
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
        User GetUser(int id);
        User GetUser(string name);
        User GetUserByEmail(string email);

        bool UserExists(int id);
        bool UserExists(string id);
        ICollection<Ticket> GetTicketsByUser(int id);
        bool CreateUser(User user);
        bool UpdateUser(User user);
        bool DeleteUser(User user);
        bool Save();
        //User logIn(string email, string password);
    }
}
