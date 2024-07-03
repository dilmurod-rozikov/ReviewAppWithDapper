using ReviewApp.Models;

namespace ReviewAppWithDapper.DTOs
{
    public class ReviewDTO
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public int Rating { get; set; }

        public int Id { get; set; }

        public ReviewDTO(string title, string description, int rating, int id)
        {
            Title = title;
            Description = description;
            Rating = rating;
            Id = id;
        }

        public Review MapToEntity()
        {
            return new() { Title = Title, Description = Description, Rating = Rating };
        }
    }
}
