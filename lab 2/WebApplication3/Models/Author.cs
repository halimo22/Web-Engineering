namespace WebApplication3.Models;

public class Author
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Biography { get; set; }
    public DateTime BirthDate { get; set; }

    public List<Book> Books { get; set; } = new List<Book>();

}