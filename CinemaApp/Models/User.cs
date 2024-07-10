using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, MinLength(6)]
        public string Password { get; set; }
        //[Required]
        //public string ConfirmPassword { get; set; }
        public DateTime BirthDate { get; set; }
        public ICollection<Ticket> Tickets { get; set; }

    }
}
