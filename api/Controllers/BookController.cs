using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fisher.Bookstore.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fisher.Bookstore.Api.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookstoreContext db;

        public BooksController(BookstoreContext db)
        {
            this.db = db;
            if (this.db.Books.Count() == 0)
            {
                this.db.Books.Add(new Book()
                {
                    Id = 1,
                    Title = "Design Patterns",
                    Author = "Erich Gamma",
                    ISBN = "978-0201633610"
                });
                this.db.Books.Add(new Book()
                {
                    Id = 2,
                    Title = "Continous Delivery",
                    Author = "Jez Humble",
                    ISBN = "978-0321601919"
                });
                this.db.Books.Add(new Book()
                {
                    Id = 3,
                    Title = "The DevOps Handbook",
                    Author = "Gene Kim",
                    ISBN = "978-1942788003"
                });
            }
            this.db.SaveChanges();
        }
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(db.Books);
    }

    [HttpGet("{id}", Name = "GetBook")]
    public IActionResult GetBook(int id)
    {
        // Try to find the correct book
        var book = this.db.Books.FirstOrDefault(b => b.Id == id);

        // If no book is found with the given ID, return HTTP 404 Not Found
        if (book == null)
        {
            return NotFound();
        }

        // Return the Book inside HTTP 200 OK
        return Ok(book);
    }

    [HttpPost]
    public IActionResult Post([FromBody] Book book)
    {
        if (book == null)
        {
            return BadRequest();
        }

        this.db.Books.Add(book);
        this.db.SaveChanges();

        return CreatedAtRoute("GetBook", new { Id = book.Id }, book);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] Book book)
    {
        // Validate the incoming book
        if (book == null || book.Id != id)
        {
            return BadRequest();
        }

        // Verify that the book is in the database
        var bookToEdit = this.db.Books.FirstOrDefault(b => b.Id == id);
        if (bookToEdit == null)
        {
            return NotFound();
        }

        bookToEdit.Title = book.Title;
        bookToEdit.ISBN = book.ISBN;

        this.db.Books.Update(bookToEdit);
        this.db.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var book = this.db.Books.FirstOrDefault(b => b.Id == id);
        if (book == null)
        {
            return NotFound();
        }

        this.db.Books.Remove(book);
        this.db.SaveChanges();

        return NoContent();
    }}

    
}