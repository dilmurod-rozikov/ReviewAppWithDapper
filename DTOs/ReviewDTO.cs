using ReviewApp.Models;

namespace ReviewAppWithDapper.DTOs
{
    public class ReviewDTO
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public int Rating { get; set; }

        public ReviewDTO(string title, string description, int rating)
        {
            Title = title;
            Description = description;
            Rating = rating;
        }

        public Review MapToEntity()
        {
            return new() { Title = Title, Description = Description, Rating = Rating };
        }
    }
}
