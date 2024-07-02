using Dapper;
using ReviewApp.DataAccess;
using ReviewApp.Interfaces;
using ReviewApp.Models;
using System.Data;

namespace ReviewApp.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly SqlDataAccess _context;
        public ReviewerRepository(SqlDataAccess context)
        {
            _context = context;
        }

        public async Task<bool> CreateReviewer(Reviewer reviewer)
        {
            using IDbConnection db = _context.CreateConnection();
            const string query = "INSERT INTO Reviewers (FirstName, LastName) VALUES (@FirstName, @LastName);";
            int rowsAffected = await db.ExecuteAsync(query, reviewer);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteReviewer(int id)
        {
            using IDbConnection db = _context.CreateConnection();
            const string query = "DELETE FROM Reviewers WHERE Id = @Id";
            int rowsAffected = await db.ExecuteAsync(query, new { Id = id });
            return rowsAffected > 0;
        }

        public async Task<Reviewer?> GetReviewer(int id)
        {
            using IDbConnection db = _context.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<Reviewer>
                ("SELECT * FROM Reviewers WHERE Id = @Id", new { Id = id });
        }

        public async Task<IEnumerable<Reviewer>> GetReviewers()
        {
            using var db = _context.CreateConnection();
            return await db.QueryAsync<Reviewer>("SELECT * FROM Reviewers");
        }

        public async Task<IEnumerable<Review>> GetReviewsByReviewer(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ReviewerExists(int reviewerId)
        {
            using var db = _context.CreateConnection();
            const string query = "SELECT * FROM Reviewers WHERE Id = @Id";
            int count = await db.ExecuteScalarAsync<int>(query, new { Id = reviewerId });
            return count > 0;
        }

        public async Task<bool> UpdateReviewer(Reviewer reviewer)
        {
            using IDbConnection db = _context.CreateConnection();
            const string query = "UPDATE Reviewers SET FirstName = @FirstName, LastName = @LastName WHERE Id = @Id";
            int rowsAffected = await db.ExecuteAsync(query, reviewer);
            return rowsAffected > 0;
        }
    }
}
