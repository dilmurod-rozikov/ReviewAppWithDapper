using ReviewApp.Models;

namespace ReviewApp.Interfaces
{
    public interface IReviewerRepository
    {
        Task<IEnumerable<Reviewer>> GetReviewers();

        Task<Reviewer?> GetReviewer(int id);

        Task<IEnumerable<Review>> GetReviewsByReviewer(int id);

        Task<bool> ReviewerExists(int reviewerId);

        Task<bool> CreateReviewer(Reviewer reviewer);

        Task<bool> UpdateReviewer(Reviewer reviewer);

        Task<bool> DeleteReviewer(int id);
    }
}
