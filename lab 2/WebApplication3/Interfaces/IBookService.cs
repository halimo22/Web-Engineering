using WebApplication3.Models;
namespace WebApplication3.Interfaces;

public interface IBookService
{
    IEnumerable<Book> GetAllBooks();
    Book? GetBookById(int id);
}
