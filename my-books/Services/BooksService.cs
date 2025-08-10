using my_books.Data;
using my_books.Data.Models;
using my_books.Data.ViewModels; // Add this using
using System.Collections.Generic;
using System.Linq;

namespace my_books.Services
{
    public class BooksService
    {
        private readonly AppDbContext _context;

        public BooksService(AppDbContext context)
        {
            _context = context;
        }

        public List<Book> GetAllBooks()
        {
            return _context.Books.ToList();
        }
        // Get Book by ID
        public Book GetBookById(int id)
        {
            return _context.Books.FirstOrDefault(b => b.BookID == id);
        }

        // Add Book using BookVM
        public void AddBook(BookVM bookVM)
        {
            var book = new Book
            {
                Title = bookVM.Title,
                Description = bookVM.Description,
                IsRead = bookVM.IsRead,
                DateRead = bookVM.IsRead ? bookVM.DateRead : null,
                Rate = bookVM.IsRead ? bookVM.Rate : null,
                Author = bookVM.Author,
                CoverUrl = bookVM.CoverUrl,
                DateAdded = DateTime.Now
            };
            _context.Books.Add(book);
            _context.SaveChanges();
        }
        // Update Book
        public void UpdateBook(int id, BookVM bookVM)
        {
            var book = _context.Books.FirstOrDefault(b => b.BookID == id);
            if (book != null)
            {
                book.Title = bookVM.Title;
                book.Description = bookVM.Description;
                book.IsRead = bookVM.IsRead;
                book.DateRead = bookVM.IsRead ? bookVM.DateRead : null;
                book.Rate = bookVM.IsRead ? bookVM.Rate : null;
                book.Author = bookVM.Author;
                book.CoverUrl = bookVM.CoverUrl;
                _context.SaveChanges();
            }
        }
        // Delete Book
        public void DeleteBook(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.BookID == id);
            if (book != null)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
            }
        }

    }
}
