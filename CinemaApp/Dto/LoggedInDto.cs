using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Dto
{
    public class LoggedInDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
    }
}
