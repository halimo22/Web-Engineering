using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models;

public class Book
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public required Author author { get; set; }
    public int PublicationYear { get; set; }
    [ForeignKey("Author")]
    public int AuthorId { get; set; }

}
