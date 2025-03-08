using System.ComponentModel.DataAnnotations;

namespace ProjectMovieBook.Data.Models.Book
{
    public class UserBookLike
    {
        [Key]
        public int Id { get; set; }

        public required string UserId { get; set; }
        public required int BookId { get; set; }

        public required AppUser User { get; set; }
        public required Book Book { get; set; }
    }
}
