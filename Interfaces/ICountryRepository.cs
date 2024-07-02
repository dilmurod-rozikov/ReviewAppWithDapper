using ReviewApp.Models;

namespace ReviewApp.Interfaces
{
    public interface ICountryRepository
    {
        public Task<IEnumerable<Country>> GetCountries();

        public Task<Country?> GetCountry(int id);

        public Task<Country?> GetCountryByOwner(int id);

        public Task<IEnumerable<Country>> GetOwnerListFromACountry(int countryId);

        public Task<bool> CountryExists(int id);

        public Task<bool> CreateCountry(Country country);

        public Task<bool> UpdateCountry(Country country);

        public Task<bool> DeleteCountry(int id);
    }
}
