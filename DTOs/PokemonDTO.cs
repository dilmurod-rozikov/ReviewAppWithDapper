using ReviewApp.Models;

namespace ReviewAppWithDapper.DTOs
{
    public class PokemonDTO
    {
        public string Name { get; set; }

        public DateOnly BirthDate { get; set; }

        public PokemonDTO(Pokemon pokemon)
        {
            Name = pokemon.Name;
            BirthDate = pokemon.BirthDate;
        }

        public Pokemon MapToEntity()
        {
            return new() { Name = Name, BirthDate = BirthDate };
        }
    }
}
