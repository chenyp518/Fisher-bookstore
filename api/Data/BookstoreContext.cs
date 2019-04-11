using Fisher.Bookstore.Data;
using Fisher.Bookstore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fisher.Bookstore.Models
{
    public class BookstoreContext : IdentityDbContext<ApplicationUser>
    {
        public BookstoreContext(DbContextOptions<BookstoreContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)=>base.OnModelCreating(builder);

        public DbSet<Book> Books {get; set;}
        public DbSet<Author> Authors {get;set;}
    }

    
}