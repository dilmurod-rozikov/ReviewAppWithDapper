using Dapper;
using ReviewApp.DataAccess;
using ReviewApp.Interfaces;
using ReviewApp.Models;
using System.Data;

namespace ReviewApp.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly SqlDataAccess _context;

        public CountryRepository(SqlDataAccess context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Country>> GetCountries()
        {
            using var db = _context.CreateConnection();
            return await db.QueryAsync<Country>("SELECT * FROM Countries");
        }

        public async Task<Country?> GetCountry(int id)
        {
            using IDbConnection db = _context.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<Country>
                ("SELECT * FROM Countries WHERE Id = @Id", new { Id = id });
        }

        public async Task<bool> CountryExists(int countryId)
        {
            using var db = _context.CreateConnection();
            const string query = "SELECT * FROM Countries WHERE Id = @Id";
            int count = await db.ExecuteScalarAsync<int>(query, new { Id = countryId });
            return count > 0;
        }

        public async Task<bool> CreateCountry(Country country)
        {
            using IDbConnection db = _context.CreateConnection();
            const string query = "INSERT INTO Countries (Name) VALUES (@Name);";
            int rowsAffected = await db.ExecuteAsync(query, country);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteCountry(int id)
        {
            using IDbConnection db = _context.CreateConnection();
            const string query = "DELETE FROM Countries WHERE Id = @Id";
            int rowsAffected = await db.ExecuteAsync(query, new { Id = id });
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateCountry(Country country)
        {
            using IDbConnection db = _context.CreateConnection();
            const string query = "UPDATE Countries SET Name = @Name WHERE Id = @Id";
            int rowsAffected = await db.ExecuteAsync(query, country);
            return rowsAffected > 0;
        }

        Task<Country?> ICountryRepository.GetCountryByOwner(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Country>> GetOwnerListFromACountry(int countryId)
        {
            throw new NotImplementedException();
        }
    }
}
