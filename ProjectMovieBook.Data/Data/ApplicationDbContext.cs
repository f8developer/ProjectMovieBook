using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectMovieBook.Models;
using ProjectMovieBook.Models.Book;
using ProjectMovieBook.Models.Movie;

namespace ProjectMovieBook.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Director> Directors { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<BookGenre> BookGenres { get; set; }  // Added BookGenre DbSet
    public DbSet<MovieGenre> MovieGenres { get; set; } // Added MovieGenre DbSet

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Book and Genre many-to-many relationship through BookGenre
        modelBuilder.Entity<BookGenre>()
            .HasKey(bg => new { bg.BookId, bg.GenreId }); // Composite key
        modelBuilder.Entity<BookGenre>()
            .HasOne(bg => bg.Book)
            .WithMany(b => b.BookGenres)
            .HasForeignKey(bg => bg.BookId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<BookGenre>()
            .HasOne(bg => bg.Genre)
            .WithMany(g => g.BookGenres)
            .HasForeignKey(bg => bg.GenreId)
            .OnDelete(DeleteBehavior.Cascade);

        // Movie and Genre many-to-many relationship through MovieGenre
        modelBuilder.Entity<MovieGenre>()
            .HasKey(mg => new { mg.MovieId, mg.GenreId }); // Composite key
        modelBuilder.Entity<MovieGenre>()
            .HasOne(mg => mg.Movie)
            .WithMany(m => m.MovieGenres)
            .HasForeignKey(mg => mg.MovieId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<MovieGenre>()
            .HasOne(mg => mg.Genre)
            .WithMany(g => g.MovieGenres)
            .HasForeignKey(mg => mg.GenreId)
            .OnDelete(DeleteBehavior.Cascade);

        // One-to-many relationship between Author and Book
        modelBuilder.Entity<Author>()
            .HasMany(a => a.Books)
            .WithOne(b => b.Author)
            .HasForeignKey(b => b.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);

        // One-to-many relationship between Director and Movie
        modelBuilder.Entity<Director>()
            .HasMany(d => d.Movies)
            .WithOne(m => m.Director)
            .HasForeignKey(m => m.DirectorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
