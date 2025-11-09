using System.ComponentModel.DataAnnotations;

namespace VelocityBoard.API.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email field is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password field is required")]
        public string Password { get; set; }
    }
}
