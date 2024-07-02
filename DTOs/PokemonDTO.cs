using ReviewApp.Models;

namespace ReviewAppWithDapper.DTOs
{
    public class PokemonDTO
    {
        public string Name { get; set; }

        public DateOnly BirthDate { get; set; }

        public PokemonDTO(string name, DateOnly birthDate)
        {
            Name = name;
            BirthDate = birthDate;
        }

        public Pokemon MapToEntity()
        {
            return new() { Name = Name, BirthDate = BirthDate };
        }
    }
}
