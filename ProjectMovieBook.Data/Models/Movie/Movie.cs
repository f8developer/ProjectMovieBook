using System.ComponentModel.DataAnnotations;

namespace ProjectMovieBook.Models.Movie
{
    public class Movie
    {
        /// <summary>
        /// The ID of the movie.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The title of the movie.
        /// </summary>
        [Required(ErrorMessage = "Movie title is required")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Movie title should be between 3 and 200 characters")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// The ID of the author of the movie.
        /// </summary>
        public int DirectorId { get; set; }

        /// <summary>
        /// The author of the movie.
        /// </summary>
        public Director Director { get; set; } = null!;

        /// <summary>
        /// The list of genres associated with the movie.
        /// </summary>
        [Required(ErrorMessage = "At least one genre must be associated with the movie")]
        public List<MovieGenre> MovieGenres { get; set; } = new();
        /// <summary>
        /// The rating of the movie (out of 5).
        /// </summary>
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; } = 1;
    }
}
