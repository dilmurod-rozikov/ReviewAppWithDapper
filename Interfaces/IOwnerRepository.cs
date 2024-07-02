using ReviewApp.Models;

namespace ReviewApp.Interfaces
{
    public interface IOwnerRepository
    {
        Task<IEnumerable<Owner>> GetOwners();

        Task<Owner?> GetOwner(int ownerId);

        Task<IEnumerable<Owner>> GetOwnersOfAPokemon(int pokemonId);

        Task<IEnumerable<Pokemon>> GetPokemonsByOwner(int ownerId);

        Task<bool> OwnerExists(int ownerId);

        Task<bool> CreateOwner(Owner owner);

        Task<bool> UpdateOwner(Owner owner);

        Task<bool> DeleteOwner(int id);
    }
}
