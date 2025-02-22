using ProjectMovieBook.Models.Book;
using ProjectMovieBook.Models.Movie;
using System.ComponentModel.DataAnnotations;

namespace ProjectMovieBook.Models
{
    public class Genre
    {
        /// <summary>
        /// The ID of the genre.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the genre.
        /// </summary>
        [Required(ErrorMessage = "Genre name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Genre name should be between 3 and 50 characters")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The list of books associated with the genre.
        /// </summary>
        public List<BookGenre> BookGenres { get; set; } = new();

        /// <summary>
        /// The list of movies associated with the genre.
        /// </summary>
        public List<MovieGenre> MovieGenres { get; set; } = new();
    }
}
