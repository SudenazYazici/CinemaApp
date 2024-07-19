using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, MinLength(6), MaxLength(30)]
        public string Password { get; set; }
        //public string ConfirmPassword { get; set; }
        public DateTime BirthDate { get; set; }
        public string Role { get; set; }
    }
}
