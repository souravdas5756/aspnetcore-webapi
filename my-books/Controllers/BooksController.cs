using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using my_books.Data.Models;
using my_books.Data.ViewModels;
using my_books.Services;

namespace my_books.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        // _booksService is a private field that holds the instance of BooksService
        private readonly BooksService _booksService;
        // Constructor that initializes the BooksService
        public BooksController(BooksService booksService)
        {
            _booksService = booksService;
        }
        // GET: api/books
        [HttpGet("get-all-books")]
        public IActionResult GetBooks(string sortBy = null)
        {
            var books = _booksService.GetAllBooks(sortBy);
            return Ok(books);
        }
        // GET: api/books/{id}
        [HttpGet("{id}")]
        public IActionResult GetBookById(int id)
        {
            var book = _booksService.GetBookById(id);
            if (book == null)
            {
                return NotFound($"Book with ID {id} not found.");
            }
            return Ok(book);
        }
        // POST: api/books
        [HttpPost("add-book")]
        public IActionResult AddBook([FromBody] BookVM book)
        {
            if (book == null)
            {
                return BadRequest("Book data is null.");
            }
            _booksService.AddBook(book);
            return Ok("Book added successfully.");
        }
        // PUT: api/books/{id}
        [HttpPut("update-book/{id}")]
        public IActionResult UpdateBook(int id, [FromBody] BookVM book)
        {
            if (book == null)
            {
                return BadRequest("Book data is null.");
            }
            var existingBook = _booksService.GetBookById(id);
            if (existingBook == null)
            {
                return NotFound($"Book with ID {id} not found.");
            }
            _booksService.UpdateBook(id, book);
            return Ok("Book updated successfully.");
        }
        // DELETE: api/books/{id}
        [HttpDelete("delete-book/{id}")]
        public IActionResult DeleteBook(int id)
        {
            var book = _booksService.GetBookById(id);
            if (book == null)
            {
                return NotFound($"Book with ID {id} not found.");
            }
            _booksService.DeleteBook(id);
            return Ok("Book deleted successfully.");
        }
    }
}
