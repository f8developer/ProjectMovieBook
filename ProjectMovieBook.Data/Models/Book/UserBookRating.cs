using System.ComponentModel.DataAnnotations;

namespace ProjectMovieBook.Data.Models.Book
{
    public class UserBookRating
    {
        [Key]
        public int Id { get; set; }

        public required string UserId { get; set; }
        public required int BookId { get; set; }

        [Range(1, 5)]
        public required int RatingValue { get; set; }

        [StringLength(500, ErrorMessage = "Message cannot exceed 500 characters")]
        public string? Message { get; set; }

        public required AppUser User { get; set; }
        public required Book Book { get; set; }
        public DateTime RatedAt { get; set; } = DateTime.Now;
    }

}
