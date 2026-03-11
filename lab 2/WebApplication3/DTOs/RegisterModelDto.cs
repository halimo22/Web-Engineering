
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.DTOs
{
    public class RegisterModelDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Range(6,32)]
        public string HashedPassword { get; set; }
        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string FirstName { get; set; }
        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string LastName { get; set; }

            [Phone]
        public string? PhoneNumber { get; set; }

    }
}