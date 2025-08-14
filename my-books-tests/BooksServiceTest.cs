using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using my_books.Data;
using my_books.Data.Models;
using my_books.Data.ViewModels;
using my_books.Services;

namespace my_books_tests;

public class BooksServiceTest
{
    //This will test the BooksService methods
    private BooksService _booksService;
    // Make _context readonly and dispose it in [OneTimeTearDown]
    private readonly AppDbContext _context;

    [OneTimeTearDown]
    // Remove the duplicate declaration of _context
    // Delete this line:
    // private readonly AppDbContext _context;
    public void OneTimeTearDown()
    {
        _context.Dispose();
    }
    //private IMapper _mapper;

    public BooksServiceTest()
    {
        // Initialize the context, logger, and mapper here
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "BooksTestDb")
            .Options;
        _context = new AppDbContext(options);
        //_mapper = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile())).CreateMapper();
        //_booksService = new BooksService(_context, _logger, _mapper);
        _booksService = new BooksService(_context);
    }

    // Example test method
    [Test]
    public void GetAllBooks_ShouldReturnAllBooks()
    {
        // Arrange

        // Act
        var result = _booksService.GetAllBooks("");
        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(4, result.Count());
    }
    [Test]
    public void GetBookById_ShouldReturnBook_WhenExists()
    {
        // Arrange


        // Act
        var result = _booksService.GetBookById(1);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("The Great Gatsby", result.Title);
    }
    [Test]
    public void GetBookById_ShouldReturnNull_WhenNotExists()
    {
        // Arrange

        // Act
        var result = _booksService.GetBookById(999);

        // Assert
        Assert.IsNull(result);
    }
    [Test]
    public void AddBook_ShouldAddBook_WhenValid()
    {
        // Arrange
        var newBook = new BookVM
        {
            Title = "New Book",
            Description = "A new book description.",
            IsRead = false,
            Author = "New Author",
            CoverUrl = "https://example.com/new-book.jpg"
        };
        // Act
        _booksService.AddBook(newBook);

        // Assert
        var result = _booksService.GetAllBooks("");
        Assert.AreEqual(5, result.Count());
        Assert.IsTrue(result.Any(b => b.Title == "New Book"));
        _booksService.DeleteBook(5);
    }
    [Test]
    public void UpdateBook_ShouldUpdateBook_WhenExists()
    {
        // Arrange
        var updatedBook = new BookVM
        {
            Title = "Updated Book",
            Description = "An updated book description.",
            IsRead = true,
            DateRead = DateTime.Now,
            Rate = 4,
            Author = "Updated Author",
            CoverUrl = "https://example.com/updated-book.jpg"
        };

        // Act
        _booksService.UpdateBook(3, updatedBook);

        // Assert
        var result = _booksService.GetBookById(3);
        Assert.IsNotNull(result);
        Assert.AreEqual("Updated Book", result.Title);
    }
    [Test]
    public void UpdateBook_ShouldNotUpdate_WhenNotExists()
    {
        // Arrange
        var updatedBook = new BookVM
        {
            Title = "Non-existent Book",
            Description = "This book does not exist.",
            IsRead = false,
            Author = "Unknown Author",
            CoverUrl = "https://example.com/non-existent-book.jpg"
        };

        // Act
        _booksService.UpdateBook(999, updatedBook);

        // Assert
        var result = _booksService.GetBookById(999);
        Assert.IsNull(result);
    }
    [Test]
    public void DeleteBook_ShouldDeleteBook_WhenExists()
    {
        // Arrange

        // Act
        var prevCount = _booksService.GetAllBooks("").Count;
        _booksService.DeleteBook(prevCount - 1);

        // Assert

        Assert.AreEqual(prevCount - 1, _booksService.GetAllBooks("").Count);
    }
    [Test]
    public void DeleteBook_ShouldNotDelete_WhenNotExists()
    {
        // Arrange

        // Act
        _booksService.DeleteBook(999);

        // Assert
        var result = _booksService.GetAllBooks("");
        Assert.AreEqual(4, result.Count());
    }

    [SetUp]
    public void Setup()
    {
        SeedDatabase();

    }

    private void SeedDatabase()
    {
        if (!_context.Books.Any())
        {
            _context.Books.AddRange(
                new Book
                {
                    Title = "The Great Gatsby",
                    Description = "A novel set in the Roaring Twenties.",
                    IsRead = true,
                    DateRead = DateTime.Now.AddDays(-10),
                    Rate = 5,
                    Author = "F. Scott Fitzgerald",
                    CoverUrl = "https://example.com/great-gatsby.jpg",
                    DateAdded = DateTime.Now
                },
                new Book
                {
                    Title = "1984",
                    Description = "A dystopian novel by George Orwell.",
                    IsRead = false,
                    Author = "George Orwell",
                    CoverUrl = "https://example.com/1984.jpg",
                    DateAdded = DateTime.Now
                },
                new Book
                {
                    Title = "To Kill a Mockingbird",
                    Description = "A novel about racial injustice in the Deep South.",
                    IsRead = true,
                    DateRead = DateTime.Now.AddDays(-5),
                    Rate = 4,
                    Author = "Harper Lee",
                    CoverUrl = "https://example.com/to-kill-a-mockingbird.jpg",
                    DateAdded = DateTime.Now
                },
                new Book
                {
                    Title = "Pride and Prejudice",
                    Description = "A romantic novel by Jane Austen.",
                    IsRead = true,
                    DateRead = DateTime.Now.AddDays(-20),
                    Rate = 5,
                    Author = "Jane Austen",
                    CoverUrl = "https://example.com/pride-and-prejudice.jpg",
                    DateAdded = DateTime.Now
                }
            );
            _context.SaveChanges();
        }

    }
}
