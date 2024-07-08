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

        public async Task<Pokemon?> GetPokemon(int id)
        {
            using IDbConnection db = _context.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<Pokemon>
                ("SELECT * FROM Pokemons WHERE Id = @Id", new { Id = id });
        }

        public async Task<IEnumerable<Pokemon>> GetPokemons()
        {
            using var db = _context.CreateConnection();
            return await db.QueryAsync<Pokemon>("SELECT * FROM Pokemons");
        }

        public async Task<double> GetPokemonRating(int id)
        {
            using IDbConnection db = _context.CreateConnection();
            const string sql = @"SELECT p.Id AS PokemonId, r.Id AS ReviewId, r.Title, r.Description, r.Rating 
                FROM Pokemons p 
                JOIN Reviews r ON r.PokemonId = p.Id 
                WHERE p.Id = @PokemonId";

            var pokemonDictionary = new Dictionary<int, Pokemon>();
            await db.QueryAsync<Pokemon, Review, Pokemon>(
                sql,
                (pokemon, review) =>
                {
                    if (!pokemonDictionary.TryGetValue(pokemon.Id, out var existingPokemon))
                    {
                        existingPokemon = pokemon;
                        existingPokemon.Reviews = new List<Review>();
                        pokemonDictionary.Add(existingPokemon.Id, existingPokemon);
                    }
                    existingPokemon.Reviews.Add(review);
                    return existingPokemon;
                },
                new { PokemonId = id },
                splitOn: "ReviewId"
            );
            var pokemon = pokemonDictionary.Values.FirstOrDefault();

            if (pokemon != null && pokemon.Reviews.Any())
            {
                return pokemon.Reviews.Average(review => review.Rating);
            }

            return 0;
        }

        public async Task<bool> PokemonExists(int id)
        {
            using var db = _context.CreateConnection();
            const string query = "SELECT 1 FROM Pokemons WHERE Id = @Id";
            var pokemon = await db.QueryFirstOrDefaultAsync<Pokemon>(query, new { Id = id });
            return pokemon != default;
        }

        public async Task<bool> CreatePokemon(Pokemon pokemon)
        {
            using IDbConnection db = _context.CreateConnection();

            const string query = "INSERT INTO Pokemons (Name, BirthDate) VALUES (@Name, @BirthDate)";
            int rowsAffected = await db.ExecuteAsync(query, pokemon);
            return rowsAffected > 0;
        }

        public async Task<bool> DeletePokemon(int id)
        {
            using IDbConnection db = _context.CreateConnection();
            const string query = "DELETE FROM Pokemons WHERE Id = @Id";
            int rowsAffected = await db.ExecuteAsync(query, new { Id = id });
            return rowsAffected > 0;
        }

        public async Task<bool> UpdatePokemon(Pokemon pokemon)
        {
            using IDbConnection db = _context.CreateConnection();
            const string query = "UPDATE Pokemons SET Name = @Name, BirthDate = @BirthDate WHERE Id = @Id";
            int rowsAffected = await db.ExecuteAsync(query, pokemon);
            return rowsAffected > 0;
        }
    }
}
