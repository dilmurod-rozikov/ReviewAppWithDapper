using ReviewApp.Models;

namespace ReviewApp.Interfaces
{
    public interface IPokemonRepository
    {
        Task<IEnumerable<Pokemon>> GetPokemons();

        Task<Pokemon?> GetPokemon(int id);

        Task<decimal> GetPokemonRating(int id);

        Task<bool> PokemonExists(int id);

        Task<bool> CreatePokemon(int ownerId, int categoryId, Pokemon pokemon);

        Task<bool> UpdatePokemon(int ownerId, int categoryId, Pokemon pokemon);

        Task<bool> DeletePokemon(int id);
    }
}
