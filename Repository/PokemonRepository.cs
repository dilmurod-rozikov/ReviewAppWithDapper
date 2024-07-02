using Dapper;
using ReviewApp.DataAccess;
using ReviewApp.Interfaces;
using ReviewApp.Models;
using System.Data;

namespace ReviewApp.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly SqlDataAccess _context;
        public PokemonRepository(SqlDataAccess context)
        {
            _context = context;
        }

        public async Task<bool> CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            using IDbConnection db = _context.CreateConnection();

            var owner = await db.QueryFirstOrDefaultAsync<Owner>
                ("SELECT * FROM Owners WHERE Id = @Id", new { Id = ownerId });
            var category = await db.QueryFirstOrDefaultAsync<Category>
                ("SELECT * FROM Categories WHERE Id = @Id", new { Id = categoryId });

            var pokemonOwner = new PokemonOwner()
            {
                Owner = owner,
                Pokemon = pokemon,
            };

            var pokemonCategory = new PokemonCategory()
            {
                Category = category,
                Pokemon = pokemon
            };
            string query = "INSERT INTO PokemonOwners (Category, Owner) VALUES (@Category, @Owner);";
            await db.ExecuteScalarAsync<int>(query, pokemonOwner);

            query = "INSERT INTO PokemonCategories (Category, Pokemon) VALUES (@Category, @Pokemon);";
            await db.ExecuteScalarAsync<int>(query, pokemonCategory);

            query = "INSERT INTO Pokemons (Name, BirthDate) VALUES (@Name, @BirthDate);";
            int rowsAffected = await db.ExecuteAsync(query, category);
            return rowsAffected > 0;
        }

        public async Task<bool> DeletePokemon(int id)
        {
            using IDbConnection db = _context.CreateConnection();
            const string query = "DELETE FROM Pokemons WHERE Id = @Id";
            int rowsAffected = await db.ExecuteAsync(query, new { Id = id });
            return rowsAffected > 0;
        }

        public async Task<Pokemon?> GetPokemon(int id)
        {
            using IDbConnection db = _context.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<Pokemon>
                ("SELECT * FROM Pokemons WHERE Id = @Id", new { Id = id });
        }

        public async Task<decimal> GetPokemonRating(int id)
        {
            using IDbConnection db = _context.CreateConnection();
            var pokemon =  await db.QueryFirstOrDefaultAsync<Pokemon>
                ("SELECT * FROM Pokemons WHERE Id = @Id", new { Id = id });
            return pokemon.Reviews.Sum(x => x.Rating)/pokemon.Reviews.Count;
        }

        public async Task<IEnumerable<Pokemon>> GetPokemons()
        {
            using var db = _context.CreateConnection();
            return await db.QueryAsync<Pokemon>("SELECT * FROM Pokemons");
        }

        public async Task<bool> PokemonExists(int id)
        {
            using var db = _context.CreateConnection();
            const string query = "SELECT * FROM Pokemons WHERE Id = @Id";
            int count = await db.ExecuteScalarAsync<int>(query, new { Id = id });
            return count > 0;
        }

        public async Task<bool> UpdatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            using IDbConnection db = _context.CreateConnection();
            const string query = "UPDATE Pokemons SET Name = @Name, BirthDate = @BirthDate WHERE Id = @Id";
            int rowsAffected = await db.ExecuteAsync(query, pokemon);
            return rowsAffected > 0;
        }
    }
}
