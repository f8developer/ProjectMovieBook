using System.ComponentModel.DataAnnotations;

namespace ProjectMovieBook.Models.Movie
{
    public class Director
    {
        /// <summary>
        /// The ID of the director.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the director.
        /// </summary>
        [Required(ErrorMessage = "Director's name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Director's name should be between 3 and 100 characters")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The list of movies directed by the director.
        /// </summary>
        public List<Movie> Movies { get; set; } = new();
    }
}