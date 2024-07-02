using ReviewApp.Models;

namespace ReviewAppWithDapper.DTOs
{
    public class CountryDTO
    {
        public string Name { get; set; }

        public CountryDTO(Country country)
        {
            Name = country.Name;
        }

        public Country MapToEntity()
        {
            return new Country { Name = Name };
        }
    }
}
