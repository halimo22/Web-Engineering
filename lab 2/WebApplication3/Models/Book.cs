
namespace WebApplication3.Models;

public class Book
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public required string Author { get; set; }
    public int PublicationYear { get; set; }
}
