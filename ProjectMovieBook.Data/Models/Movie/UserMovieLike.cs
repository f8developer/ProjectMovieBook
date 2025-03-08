using System.ComponentModel.DataAnnotations;

namespace ProjectMovieBook.Data.Models.Movie
{
    public class UserMovieLike
    {
        [Key]
        public int Id { get; set; }

        public required string UserId { get; set; }
        public required int MovieId { get; set; }

        public required AppUser User { get; set; }
        public required Movie Movie { get; set; }
    }
}
