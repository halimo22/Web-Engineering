using WebApplication3.DTOs;
using WebApplication3.Models;
namespace WebApplication3.Interfaces;

public interface IBookService
{
    Task<IEnumerable<Book>> GetAllBooks();
    Task<BookDetailsDTO?> GetBookById(int id);

    Task<Book> AddBook(Book book);
    Task<Book?> UpdateBook(int id, Book updatedBook);
}
