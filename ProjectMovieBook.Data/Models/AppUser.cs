using Microsoft.AspNetCore.Identity;
using ProjectMovieBook.Data.Models.Book;
using ProjectMovieBook.Data.Models.Movie;

namespace ProjectMovieBook.Data.Models
{
    public class AppUser : IdentityUser
    {
        [PersonalData]
        public string? FirstName { get; set; }

        [PersonalData]
        public string? LastName { get; set; }

        public string? Nickname { get; set; }

        // Ratings
        public List<UserBookRating>? BookRatings { get; set; }
        public List<UserMovieRating>? MovieRatings { get; set; }

        // Likes
        public List<UserBookLike>? BookLikes { get; set; }
        public List<UserMovieLike>? MovieLikes { get; set; }
    }
}
