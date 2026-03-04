using Microsoft.AspNetCore.Mvc;
using WebApplication3.Interfaces;

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

    [HttpGet]
    public IActionResult GetAllBooks()
    {
        var books = bookService.GetAllBooks();
        return Ok(books);

    }

    [HttpGet("{id}")]
    public IActionResult GetBookById(int id)
    {
        var book = bookService.GetBookById(id);
        if (book == null)
        {
            return NotFound();
        }
        return Ok(book);
    }
}
