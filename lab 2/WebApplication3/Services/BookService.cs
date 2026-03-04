using Microsoft.EntityFrameworkCore;
using WebApplication3.Database;
using WebApplication3.Interfaces;
using WebApplication3.Models;
using WebApplication3.DTOs;
namespace WebApplication3.Services;

public class BookService : IBookService
{
    ApplicationDbContext _context;
    
    public BookService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Book>> GetAllBooks()
    {
        return await _context.Books.ToListAsync();
    }

    public async Task<BookDetailsDTO?> GetBookById(int id)
    {
        return await _context.Books.Where(b => b.Id == id).Include(b => b.author).AsNoTracking().Select(b => new BookDetailsDTO
        {
            Id = b.Id,
            Title = b.Title,
            AuthorName = b.author.Name,
            PublicationYear = b.PublicationYear
        }).FirstOrDefaultAsync();

    }


    public async Task<Book> AddBook(Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task<Book?> UpdateBook(int id, Book updatedBook)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null)
        {
            return null;
        }
        book.Title = updatedBook.Title;
        book.author = updatedBook.author;
        book.PublicationYear = updatedBook.PublicationYear;
        await _context.SaveChangesAsync();
        return book;
    }
    public async Task DeleteBook(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book != null)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
    
    }

    public async Task<IEnumerable<Book>> GetBooksByAuthorId(int authorId)
    {
        return await _context.Books.Where(b => b.AuthorId == authorId).ToListAsync();
    }

    public async Task<AuthorNameDTO> GetAuthorName(int bookId)
    {
        return await _context.Books.Where(b => b.Id == bookId).Include(b => b.author).AsNoTracking().Select(b => new AuthorNameDTO
        {
            Name = b.author.Name
        }).FirstOrDefaultAsync();
    }
}
