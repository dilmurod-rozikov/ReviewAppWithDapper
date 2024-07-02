using Dapper;
using ReviewApp.DataAccess;
using ReviewApp.Interfaces;
using ReviewApp.Models;
using System.Data;

namespace ReviewApp.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly SqlDataAccess _context;
        public CategoryRepository(SqlDataAccess context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            using var db = _context.CreateConnection();
            return await db.QueryAsync<Category>("SELECT * FROM Categories");
        }

        public async Task<Category?> GetCategory(int id)
        {
            using IDbConnection db = _context.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<Category>
                ("SELECT * FROM Categories WHERE Id = @Id", new { Id = id });
        }

        public async Task<bool> CategoryExists(int categoryId)
        {
            using var db = _context.CreateConnection();
            const string query = "SELECT * FROM Categories WHERE Id = @Id";
            int count = await db.ExecuteScalarAsync<int>(query, new { Id = categoryId });
            return count > 0;
        }

        public async Task<bool> CreateCategory(Category category)
        {
            using IDbConnection db = _context.CreateConnection();
            const string query = "INSERT INTO Categories (Name) VALUES (@Name);";
            int rowsAffected = await db.ExecuteAsync(query, category);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteCategory(int id)
        {
            using IDbConnection db = _context.CreateConnection();
            const string query = "DELETE FROM Categories WHERE Id = @Id";
            int rowsAffected = await db.ExecuteAsync(query, new { Id = id });
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateCategory(Category category)
        {
            using IDbConnection db = _context.CreateConnection();
            const string query = "UPDATE Categories SET Name = @Name WHERE Id = @Id";
            int rowsAffected = await db.ExecuteAsync(query, category);
            return rowsAffected > 0;
        }
    }
}
