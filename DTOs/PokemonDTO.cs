using ReviewApp.Models;

namespace ReviewAppWithDapper.DTOs
{
    public class PokemonDTO
    {
        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        public int Id { get; set; }

        public PokemonDTO(string name, DateTime birthDate, int id)
        {
            Name = name;
            BirthDate = birthDate;
            Id = id;
        }

        public Pokemon MapToEntity()
        {
            return new() { Name = Name, BirthDate = BirthDate };
        }
    }
}
