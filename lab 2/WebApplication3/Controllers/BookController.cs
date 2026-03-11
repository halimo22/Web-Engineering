using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.DTOs;
using WebApplication3.Interfaces;
using WebApplication3.Models;

namespace WebApplication3.Controllers;
[Route("books")]
[ApiController]

public class BookController : ControllerBase
{
    IBookService bookService;

    public BookController(IBookService bookService)
    {
        this.bookService = bookService;
    }
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllBooks()
    {
        var books = await bookService.GetAllBooks();
        return Ok(books);

    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBookById(int id)
    {
        BookDetailsDTO? book = await bookService.GetBookById(id);
        if (book == null)
        {
            return NotFound();
        }
        return Ok(book);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddBook(Book book)
    {
        var addedBook = await bookService.AddBook(book);
        return CreatedAtAction(nameof(GetBookById), new { id = addedBook.Id }, addedBook);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateBook(int id, Book updatedBook)
    {
        var book = await bookService.UpdateBook(id, updatedBook);
        if (book == null)
        {
            return NotFound();
        }
        return Ok(book);
    }
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteBook(int id)
    {
        await bookService.DeleteBook(id);
        return NoContent();
    }
    [HttpGet("author/{authorId}")]
    [Authorize]
    public async Task<IActionResult> GetBooksByAuthor(int authorId)
    {
        var books = await bookService.GetBooksByAuthorId(authorId);
        return Ok(books);
    }

    [HttpGet("{id}/author")]
    [Authorize]
    public async Task<IActionResult> GetAuthorName(int id)
    {
        var author = await bookService.GetAuthorName(id);
        if (author == null)
        {
            return NotFound();
        }
        return Ok(author);
    }
}
