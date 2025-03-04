// ProjectMovieBook.Data.Tests/MovieRepositoryTests.cs
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ProjectMovieBook.Data;
using ProjectMovieBook.Models;
using ProjectMovieBook.Models.Book;
using ProjectMovieBook.Models.Movie;

[TestFixture]
public class MovieRepositoryTests
{
    private ApplicationDbContext _context;
    private Director _testDirector;
    private Author _testAuthor;
    private Genre _testGenre;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;

        _context = new ApplicationDbContext(options);

        // Seed required entities
        _testDirector = new Director { Name = "Christopher Nolan" };
        _testAuthor = new Author { Name = "Harper Lee" };
        _testGenre = new Genre { Name = "Sci-Fi" };

        _context.AddRange(_testDirector, _testAuthor, _testGenre);
        _context.SaveChanges();
    }

    [Test]
    public async Task TestAddMovie()
    {
        // Arrange
        var movie = new Movie
        {
            Title = "Interstellar",
            DirectorId = _testDirector.Id,
            Rating = 5,
            MovieGenres = new List<MovieGenre>
            {
                new() { GenreId = _testGenre.Id }
            }
        };

        // Act
        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();

        // Assert
        var savedMovie = await _context.Movies
            .Include(m => m.MovieGenres)
            .FirstOrDefaultAsync(m => m.Title == "Interstellar");

        Assert.Multiple(() =>
        {
            Assert.That(savedMovie, Is.Not.Null);
            Assert.That(savedMovie.MovieGenres, Has.Count.EqualTo(1));
            Assert.That(savedMovie.MovieGenres.First().GenreId, Is.EqualTo(_testGenre.Id));
            Assert.That(savedMovie.DirectorId, Is.EqualTo(_testDirector.Id));
        });
    }

    [Test]
    public async Task TestAddBook()
    {
        // Arrange
        var book = new Book
        {
            Title = "To Kill a Mockingbird",
            AuthorId = _testAuthor.Id,
            Rating = 5,
            BookGenres = new List<BookGenre>
            {
                new() { GenreId = _testGenre.Id }
            }
        };

        // Act
        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        // Assert
        var savedBook = await _context.Books
            .Include(b => b.BookGenres)
            .FirstOrDefaultAsync(b => b.Title == "To Kill a Mockingbird");

        Assert.Multiple(() =>
        {
            Assert.That(savedBook, Is.Not.Null);
            Assert.That(savedBook.BookGenres, Has.Count.EqualTo(1));
            Assert.That(savedBook.BookGenres.First().GenreId, Is.EqualTo(_testGenre.Id));
            Assert.That(savedBook.AuthorId, Is.EqualTo(_testAuthor.Id));
        });
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}