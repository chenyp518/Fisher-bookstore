using Microsoft.EntityFrameworkCore;

namespace Fisher.Bookstore.Models
{
    public class BookstoreContext : DbContext
    {
        public BookstoreContext(DdContextOptions<BookstoreContext> options)
            : base(options)
        {
        }

        public DdSet<Book> Books {get; set;}
    }
}