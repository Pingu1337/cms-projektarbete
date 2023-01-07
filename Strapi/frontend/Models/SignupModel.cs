using System.ComponentModel.DataAnnotations;

namespace frontend.Models
{
    public class SignupModel
    {
        [Required]
        [StringLength(20, ErrorMessage = "Name is too long.")]
        public string? Username { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email adress is invalid")]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }

        public string Role { get; set; } = "1";
    }   
}
