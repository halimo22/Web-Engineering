
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.DTOs
{
    public class RegisterModelDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string HashedPassword { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}