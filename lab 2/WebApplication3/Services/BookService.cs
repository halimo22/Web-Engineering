using WebApplication3.Interfaces;
using WebApplication3.Models;
namespace WebApplication3.Services;

public class BookService : IBookService
{
    List<Book> books = new List<Book>
    {
        new Book { Id = 1, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", PublicationYear = 1925 },
        new Book { Id = 2, Title = "To Kill a Mockingbird", Author = "Harper Lee", PublicationYear = 1960 },
        new Book { Id = 3, Title = "1984", Author = "George Orwell", PublicationYear = 1949 }
    };
    
    public IEnumerable<Book> GetAllBooks()
    {
        return books;
    }

    public Book? GetBookById(int id)
    {
        return books.FirstOrDefault(b => b.Id == id);
    }
}
