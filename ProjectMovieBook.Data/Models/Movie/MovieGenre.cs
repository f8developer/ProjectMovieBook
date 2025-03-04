namespace ProjectMovieBook.Models.Movie
{
    public class MovieGenre
    {
        /// <summary>
        /// The ID of the movie.
        /// </summary>
        public int MovieId { get; set; }

        /// <summary>
        /// The movie associated with the genre.
        /// </summary>
        public Movie Movie { get; set; } = null!;

        /// <summary>
        /// The ID of the genre.
        /// </summary>
        public int GenreId { get; set; }

        /// <summary>
        /// The genre associated with the movie.
        /// </summary>
        public Genre Genre { get; set; } = null!;
    }
}
