using ReviewApp.Models;

namespace ReviewAppWithDapper.DTOs
{
    public class ReviewDTO
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public int Rating { get; set; }

        public ReviewDTO(Review review)
        {
            Title = review.Title;
            Description = review.Description;
            Rating = review.Rating;
        }

        public Review MapToEntity()
        {
            return new() { Title = Title, Description = Description, Rating = Rating };
        }
    }
}
