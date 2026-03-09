using System.ComponentModel.DataAnnotations;

namespace WebApplication3.DTOs
{
    public class LoginModelDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string HashedPassword { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }
        
    }
}