using Dapper;
using ReviewApp.DataAccess;
using ReviewApp.Interfaces;
using ReviewApp.Models;
using System.Data;

namespace ReviewApp.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly SqlDataAccess _context;
        public OwnerRepository(SqlDataAccess context)
        {
            _context = context;
        }

        public async Task<Owner?> GetOwner(int id)
        {
            using IDbConnection db = _context.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<Owner>
                ("SELECT * FROM Owners WHERE Id = @Id", new { Id = id });
        }

        public async Task<IEnumerable<Owner>> GetOwners()
        {
            using var db = _context.CreateConnection();
            return await db.QueryAsync<Owner>("SELECT * FROM Owners");
        }

        public async Task<bool> OwnerExists(int ownerId)
        {
            using var db = _context.CreateConnection();
            const string query = "SELECT 1 FROM Owners WHERE Id = @Id";
            var owner = await db.QueryFirstOrDefaultAsync<Owner>(query, new { Id = ownerId });
            return owner != default;
        }

        public async Task<bool> CreateOwner(Owner owner)
        {
            using IDbConnection db = _context.CreateConnection();
            const string query = "INSERT INTO Owners (Name, Gym) VALUES (@Name, @Gym);";
            int rowsAffected = await db.ExecuteAsync(query, owner);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteOwner(int id)
        {
            using IDbConnection db = _context.CreateConnection();
            const string query = "DELETE FROM Owners WHERE Id = @Id";
            int rowsAffected = await db.ExecuteAsync(query, new { Id = id });
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateOwner(Owner owner)
        {
            using IDbConnection db = _context.CreateConnection();
            const string query = "UPDATE Owners SET Name = @Name, Gym = @Gym WHERE Id = @Id";
            int rowsAffected = await db.ExecuteAsync(query, owner);
            return rowsAffected > 0;
        }
    }
}
