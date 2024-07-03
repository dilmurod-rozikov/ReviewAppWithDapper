using Dapper;
using ReviewApp.DataAccess;
using ReviewApp.Interfaces;
using ReviewApp.Models;
using System.Data;

namespace ReviewApp.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly SqlDataAccess _context;
        public ReviewRepository(SqlDataAccess context)
        {
            _context = context;
        }

        public async Task<Review?> GetReview(int id)
        {
            using IDbConnection db = _context.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<Review>
                ("SELECT * FROM Reviews WHERE Id = @Id", new { Id = id });
        }

        public async Task<IEnumerable<Review>> GetReviews()
        {
            using var db = _context.CreateConnection();
            return await db.QueryAsync<Review>("SELECT * FROM Reviews");
        }

        public async Task<bool> ReviewExists(int reviewerId)
        {
            using var db = _context.CreateConnection();
            const string query = "SELECT 1 FROM Reviews WHERE Id = @Id";
            var review = await db.QueryFirstOrDefaultAsync<Review>(query, new { Id = reviewerId });
            return review != default;
        }

        public async Task<bool> CreateReview(Review review)
        {
            using IDbConnection db = _context.CreateConnection();
            const string query = "INSERT INTO Reviews (Title, Description, Rating) VALUES (@Title, @Description, @Rating)";
            int rowsAffected = await db.ExecuteAsync(query, review);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteReview(int id)
        {
            using IDbConnection db = _context.CreateConnection();
            const string query = "DELETE FROM Reviews WHERE Id = @Id";
            int rowsAffected = await db.ExecuteAsync(query, new { Id = id });
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateReview(Review review)
        {
            using IDbConnection db = _context.CreateConnection();
            const string query = "UPDATE Reviews SET Title = @Title, Description = @Description, Rating = @Rating WHERE Id = @Id";
            int rowsAffected = await db.ExecuteAsync(query, review);
            return rowsAffected > 0;
        }

        public Task<IEnumerable<Review>> GetReviewsOfAPokemon(int pokemonId)
        {
            throw new NotImplementedException();
        }
    }
}
