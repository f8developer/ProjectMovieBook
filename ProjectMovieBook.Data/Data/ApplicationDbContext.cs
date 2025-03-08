using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectMovieBook.Data.Models;
using ProjectMovieBook.Data.Models.Book;
using ProjectMovieBook.Data.Models.Movie;
using ProjectMovieBook.Models;

namespace ProjectMovieBook.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser>
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
    public DbSet<BookGenre> BookGenres { get; set; }
    public DbSet<MovieGenre> MovieGenres { get; set; }
    public DbSet<UserBookRating> UserBookRatings { get; set; }
    public DbSet<UserMovieRating> UserMovieRatings { get; set; }
    public DbSet<UserBookLike> UserBookLikes { get; set; }
    public DbSet<UserMovieLike> UserMovieLikes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Book relationships
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasOne(b => b.CreatedByUser)
                .WithMany()
                .HasForeignKey(b => b.CreatedByUserId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasMany(b => b.UserRatings)
                .WithOne(r => r.Book)
                .HasForeignKey(r => r.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(b => b.UserLikes)
                .WithOne(l => l.Book)
                .HasForeignKey(l => l.BookId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Movie relationships
        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasOne(m => m.CreatedByUser)
                .WithMany()
                .HasForeignKey(m => m.CreatedByUserId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasMany(m => m.UserRatings)
                .WithOne(r => r.Movie)
                .HasForeignKey(r => r.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(m => m.UserLikes)
                .WithOne(l => l.Movie)
                .HasForeignKey(l => l.MovieId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure User relationships
        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasMany(u => u.BookRatings)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(u => u.MovieRatings)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(u => u.BookLikes)
                .WithOne(l => l.User)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(u => u.MovieLikes)
                .WithOne(l => l.User)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure UserMovieRating
        modelBuilder.Entity<UserMovieRating>(entity =>
        {
            // Primary key configuration
            entity.HasKey(e => e.Id);

            // Relationships
            entity.HasOne(e => e.User)
                .WithMany(u => u.MovieRatings)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Movie)
                .WithMany(m => m.UserRatings)
                .HasForeignKey(e => e.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            // Property configurations
            entity.Property(e => e.RatingValue).IsRequired();
            entity.Property(e => e.Message).HasMaxLength(500);
        });

        // Configure UserMovieLike
        modelBuilder.Entity<UserMovieLike>(entity =>
        {
            // Primary key configuration
            entity.HasKey(e => e.Id);

            // Relationships
            entity.HasOne(e => e.User)
                .WithMany(u => u.MovieLikes)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Movie)
                .WithMany(m => m.UserLikes)
                .HasForeignKey(e => e.MovieId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure many-to-many relationships
        modelBuilder.Entity<BookGenre>()
            .HasKey(bg => new { bg.BookId, bg.GenreId });

        modelBuilder.Entity<MovieGenre>()
            .HasKey(mg => new { mg.MovieId, mg.GenreId });

        // Configure other relationships
        modelBuilder.Entity<Author>()
            .HasMany(a => a.Books)
            .WithOne(b => b.Author)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Director>()
            .HasMany(d => d.Movies)
            .WithOne(m => m.Director)
            .OnDelete(DeleteBehavior.Cascade);
    }
}