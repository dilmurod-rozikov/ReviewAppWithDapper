using ReviewApp.Models;

namespace ReviewAppWithDapper.DTOs
{
    public class ReviewerDTO
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ReviewerDTO(string firstName, string lastName, int id)
        {
            FirstName = firstName;
            LastName = lastName;
            Id = id;
        }

        public Reviewer MapToEntity()
        {
            return new() { FirstName = FirstName, LastName = LastName };
        }
    }
}
