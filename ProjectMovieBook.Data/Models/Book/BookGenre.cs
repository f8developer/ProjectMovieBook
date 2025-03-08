namespace ProjectMovieBook.Data.Models.Book
{
    public class BookGenre
    {
        /// <summary>
        /// The ID of the book.
        /// </summary>
        public int BookId { get; set; }

        /// <summary>
        /// The book associated with the genre.
        /// </summary>
        public Book Book { get; set; } = null!;

        /// <summary>
        /// The ID of the genre.
        /// </summary>
        public int GenreId { get; set; }

        /// <summary>
        /// The genre associated with the book.
        /// </summary>
        public Genre Genre { get; set; } = null!;
    }
}
