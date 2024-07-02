using ReviewApp.Models;

namespace ReviewAppWithDapper.DTOs
{
    public class ReviewerDTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ReviewerDTO(Reviewer reviewer)
        {
            FirstName = reviewer.FirstName;
            LastName = reviewer.LastName;
        }

        public Reviewer MapToEntity()
        {
            return new() { FirstName = FirstName, LastName = LastName };
        }
    }
}
