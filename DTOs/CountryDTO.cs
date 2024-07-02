using ReviewApp.Models;

namespace ReviewAppWithDapper.DTOs
{
    public class CountryDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public CountryDTO(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public Country MapToEntity()
        {
            return new Country { Name = Name };
        }
    }
}
