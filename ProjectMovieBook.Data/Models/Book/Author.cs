using System.ComponentModel.DataAnnotations;

namespace ProjectMovieBook.Data.Models.Book
{
    public class Author
    {
        /// <summary>
        /// The ID of the author.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the author.
        /// </summary>
        [Required(ErrorMessage = "Author's name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Author's name should be between 3 and 100 characters")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The list of books written by the author.
        /// </summary>
        public List<Book> Books { get; set; } = new();
    }
}