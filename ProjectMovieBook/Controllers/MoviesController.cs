using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectMovieBook.Data;
using ProjectMovieBook.Data.Models;
using ProjectMovieBook.Data.Models.Movie;

namespace ProjectMovieBook.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index(string searchQuery = "")
        {
            var movies = _context.Movies.Include(m => m.Director)
                                        .Include(m => m.MovieGenres)  // Include Genres for filtering
                                        .AsQueryable();

            // Filter by title, director name, or genre name if searchQuery is provided
            if (!string.IsNullOrEmpty(searchQuery))
            {
                movies = movies.Where(m => m.Title.Contains(searchQuery) ||
                                            m.Director.Name.Contains(searchQuery) ||
                                            m.MovieGenres.Any(mg => mg.Genre.Name.Contains(searchQuery)));
            }

            ViewData["SearchQuery"] = searchQuery;

            return View(await movies.ToListAsync());
        }



        // GET: Movies/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Directors, "Id", "Name");
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Movie movie, string DirectorName, string Genres)
        {
            ModelState.Remove("Director");

            if (ModelState.IsValid)
            {
                // Handle Director
                var director = await _context.Directors.FirstOrDefaultAsync(d => d.Name == DirectorName);
                if (director == null)
                {
                    director = new Director { Name = DirectorName };
                    _context.Directors.Add(director);
                    await _context.SaveChangesAsync();
                }

                movie.DirectorId = director.Id;

                // Add the movie itself first to generate its Id
                _context.Movies.Add(movie);
                await _context.SaveChangesAsync();

                // Handle Genres
                var genreList = new List<Genre>();

                if (!string.IsNullOrWhiteSpace(Genres))
                {
                    var genreNames = Genres.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                           .Select(g => g.Trim())
                                           .Distinct();

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

                var movieGenres = genreList.Select(g => new MovieGenre { MovieId = movie.Id, GenreId = g.Id }).ToList();
                _context.AddRange(movieGenres);

                await _context.SaveChangesAsync();

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

            ViewData["DirectorId"] = new SelectList(_context.Directors, "Id", "Name", movie.DirectorId);
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                                    .Include(m => m.Director)
                                    .Include(m => m.MovieGenres)   // Include the BookGenres (many-to-many join)
                                        .ThenInclude(mg => mg.Genre) // Include the Genre from BookGenres
                                    .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            string directorName = movie.Director.Name;
            string genres = string.Join(", ", movie.MovieGenres.Select(bg => bg.Genre.Name));

            ViewData["AuthorId"] = new SelectList(_context.Directors, "Id", "Name", movie.DirectorId);

            // Pass the AuthorName and Genres as ViewData to use them in the view
            ViewData["DirectorName"] = directorName;
            ViewData["Genres"] = genres;

            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,DirectorId,Rating")] Movie movie, string DirectorName, string Genres)
        {
            ModelState.Remove("Director");

            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Handle Director
                    var director = await _context.Directors.FirstOrDefaultAsync(d => d.Name == DirectorName);
                    if (director == null)
                    {
                        director = new Director { Name = DirectorName };
                        _context.Directors.Add(director);
                        await _context.SaveChangesAsync();
                    }

                    movie.DirectorId = director.Id;

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

                    // Fetch existing genres for this movie
                    var existingGenres = await _context.MovieGenres
                                                        .Where(mg => mg.MovieId == movie.Id)
                                                        .Include(mg => mg.Genre)
                                                        .ToListAsync();

                    // Remove genres no longer associated with the movie
                    var genresToRemove = existingGenres.Where(eg => !genreList.Select(g => g.Name).Contains(eg.Genre.Name)).ToList();
                    _context.MovieGenres.RemoveRange(genresToRemove);

                    // Add new genres or genres that were not previously associated with the movie
                    foreach (var genre in genreList)
                    {
                        if (!existingGenres.Any(eg => eg.GenreId == genre.Id))
                        {
                            _context.MovieGenres.Add(new MovieGenre
                            {
                                MovieId = movie.Id,
                                GenreId = genre.Id
                            });
                        }
                    }

                    // Update the movie itself
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
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

            // Set the DirectorName and Genres properties for the view
            string directorName = movie.Director.Name;
            string genres = string.Join(", ", movie.MovieGenres.Select(mg => mg.Genre.Name));

            ViewData["DirectorId"] = new SelectList(_context.Directors, "Id", "Name", movie.DirectorId);
            ViewData["DirectorName"] = directorName;
            ViewData["Genres"] = genres;

            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.Director)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
