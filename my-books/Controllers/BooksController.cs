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
        private readonly ILogger<BooksController> _logger;
        // Constructor that initializes the BooksService
        public BooksController(BooksService booksService, ILogger<BooksController> logger)
        {
            _booksService = booksService;
            _logger = logger;

        }
        // GET: api/books
        [HttpGet("get-all-books")]
        public IActionResult GetBooks(string sortBy = null)
        {
            _logger.LogInformation("Fetching all books with sorting option: {SortBy}", sortBy);
            var books = _booksService.GetAllBooks(sortBy);
            return Ok(books);
        }
        // GET: api/books/{id}
        [HttpGet("{id}")]
        public IActionResult GetBookById(int id)
        {
            _logger.LogInformation("Fetching book with ID: {Id}", id);
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
            _logger.LogInformation("Adding a new book: {@Book}", book);
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
            _logger.LogInformation("Updating book with ID: {Id}, Data: {@Book}", id, book);
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
            _logger.LogInformation("Deleting book with ID: {Id}", id);
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
