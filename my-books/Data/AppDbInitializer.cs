using Microsoft.EntityFrameworkCore.Diagnostics;
using my_books.Data.Models;

namespace my_books.Data
{
    public class AppDbInitializer
    {
        //seed the database with initial data
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();
            // Check if any books exist
            if (!context.Books.Any())
            {
                // Add initial books
                context.Books.AddRange(
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
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
