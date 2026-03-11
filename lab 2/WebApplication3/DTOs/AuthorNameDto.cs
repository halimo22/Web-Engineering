using System.ComponentModel.DataAnnotations;
namespace WebApplication3.DTOs;

public class AuthorNameDTO
{
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
}