using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectMovieBook.Data;
using ProjectMovieBook.Models;
using ProjectMovieBook.Models.Book;

namespace ProjectMovieBook.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index(string searchQuery = "")
        {
            var books = _context.Books
                .Include(b => b.Author)       // Include the Author navigation property
                .Include(b => b.BookGenres)   // Include the BookGenres (many-to-many join)
                    .ThenInclude(bg => bg.Genre) // Include the Genre from BookGenres
                .AsQueryable();

            // Apply filter if search query is provided
            if (!string.IsNullOrEmpty(searchQuery))
            {
                books = books.Where(b => b.Title.Contains(searchQuery) ||
                                          b.Author.Name.Contains(searchQuery) ||  // Query by Author Name
                                          b.BookGenres.Any(bg => bg.Genre.Name.Contains(searchQuery))); // Query by Genre Name
            }

            ViewData["SearchQuery"] = searchQuery;

            return View(await books.ToListAsync());
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book, string AuthorName, string Genres)
        {
            ModelState.Remove("Author");

            if (ModelState.IsValid)
            {
                // Handle Author
                var author = await _context.Authors.FirstOrDefaultAsync(a => a.Name == AuthorName);
                if (author == null)
                {
                    author = new Author { Name = AuthorName };
                    _context.Authors.Add(author);
                    await _context.SaveChangesAsync();
                }

                book.AuthorId = author.Id;

                // Add the book itself first to generate its Id
                _context.Books.Add(book);
                await _context.SaveChangesAsync();

                // Handle Genres
                var genreList = new List<Genre>();

                if (!string.IsNullOrWhiteSpace(Genres))
                {
                    var genreNames = Genres.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                           .Select(g => g.Trim())
                                           .Distinct()
                                           .ToList();

                    // Get existing genres and create new ones if necessary
                    var existingGenres = await _context.Genres
                                                        .Where(g => genreNames.Contains(g.Name))
                                                        .ToListAsync();

                    // Create genres that don't exist
                    var newGenres = genreNames.Except(existingGenres.Select(g => g.Name))
                                              .Select(g => new Genre { Name = g })
                                              .ToList();

                    if (newGenres.Any())
                    {
                        _context.Genres.AddRange(newGenres);
                        await _context.SaveChangesAsync(); // Save new genres to database
                    }

                    // Combine the existing and new genres
                    genreList.AddRange(existingGenres);
                    genreList.AddRange(newGenres);
                }

                // Add the BookGenres join entities to establish the many-to-many relationship
                var bookGenres = genreList.Select(g => new BookGenre { BookId = book.Id, GenreId = g.Id }).ToList();
                _context.AddRange(bookGenres); // Add the BookGenre entries to the context

                await _context.SaveChangesAsync(); // Save all changes at once

                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Handle validation errors
                var errors = string.Join(", ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));

                Console.WriteLine("Validation Errors: " + errors);
            }

            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name", book.AuthorId);
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                                    .Include(b => b.Author)
                                    .Include(b => b.BookGenres)   // Include the BookGenres (many-to-many join)
                                        .ThenInclude(bg => bg.Genre) // Include the Genre from BookGenres
                                    .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            // Set the AuthorName and Genres properties
            string authorName = book.Author.Name;
            string genres = string.Join(", ", book.BookGenres.Select(bg => bg.Genre.Name));

            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name", book.AuthorId);

            // Pass the AuthorName and Genres as ViewData to use them in the view
            ViewData["AuthorName"] = authorName;
            ViewData["Genres"] = genres;

            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,AuthorId,Rating")] Book book, string AuthorName, string Genres)
        {
            ModelState.Remove("Author");

            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Handle Author
                    var author = await _context.Authors.FirstOrDefaultAsync(a => a.Name == AuthorName);
                    if (author == null)
                    {
                        author = new Author { Name = AuthorName };
                        _context.Authors.Add(author);
                        await _context.SaveChangesAsync();
                    }

                    book.AuthorId = author.Id;

                    // Handle Genres
                    var genreList = new List<Genre>();

                    if (!string.IsNullOrWhiteSpace(Genres))
                    {
                        var genreNames = Genres.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                               .Select(g => g.Trim())
                                               .Distinct();

                        foreach (var genreName in genreNames)
                        {
                            var genre = await _context.Genres.FirstOrDefaultAsync(g => g.Name == genreName);
                            if (genre == null)
                            {
                                genre = new Genre { Name = genreName };
                                _context.Genres.Add(genre);
                                await _context.SaveChangesAsync();  // Save for each new genre, but only if needed
                            }

                            genreList.Add(genre);
                        }
                    }

                    // Fetch existing genres for this book
                    var existingGenres = await _context.BookGenres
                                                        .Where(bg => bg.BookId == book.Id)
                                                        .Include(bg => bg.Genre)
                                                        .ToListAsync();

                    // Remove genres no longer associated with the book
                    var genresToRemove = existingGenres.Where(eg => !genreList.Select(g => g.Name).Contains(eg.Genre.Name)).ToList();
                    _context.BookGenres.RemoveRange(genresToRemove);

                    // Add new genres or genres that were not previously associated with the book
                    foreach (var genre in genreList)
                    {
                        if (!existingGenres.Any(eg => eg.GenreId == genre.Id))
                        {
                            _context.BookGenres.Add(new BookGenre
                            {
                                BookId = book.Id,
                                GenreId = genre.Id
                            });
                        }
                    }

                    // Update the book itself
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // Set the AuthorName and Genres properties for the view
            string authorName = book.Author.Name;
            string genres = string.Join(", ", book.BookGenres.Select(bg => bg.Genre.Name));

            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name", book.AuthorId);
            ViewData["AuthorName"] = authorName;
            ViewData["Genres"] = genres;

            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
