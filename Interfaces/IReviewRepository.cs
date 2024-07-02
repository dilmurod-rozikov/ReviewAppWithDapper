using ReviewApp.Models;

namespace ReviewApp.Interfaces
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetReviewsOfAPokemon(int pokemonId);

        Task<IEnumerable<Review>> GetReviews();

        Task<Review?> GetReview(int id);

        Task<bool> ReviewExists(int id);

        Task<bool> CreateReview(Review review);

        Task<bool> UpdateReview(Review review);

        Task<bool> DeleteReview(int id);
    }
}
