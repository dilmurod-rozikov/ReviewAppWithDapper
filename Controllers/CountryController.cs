using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ReviewApp.Interfaces;
using ReviewApp.Models;
using ReviewAppWithDapper.DTOs;

namespace ReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : Controller
    {
        private readonly ICountryRepository _countryRepositry;
        private readonly IOwnerRepository _ownerRepository;
        public CountryController(
            ICountryRepository countryRepositry,
            IOwnerRepository ownerRepository)
        {
            _countryRepositry = countryRepositry;
            _ownerRepository = ownerRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<CountryDTO>>> GetCountries()
        {

            var countries = await _countryRepositry.GetCountries();
            var countryDTOs = countries
                .Select(country => new CountryDTO(country.Id, country.Name))
                .ToList();

            return Ok(countryDTOs);
        }

        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<CountryDTO>> GetCountry(int countryId)
        {
            if (! await _countryRepositry.CountryExists(countryId))
                return NotFound();

            var country = await
                _countryRepositry.GetCountry(countryId);

            var countryDTO = new CountryDTO(country.Id, country.Name);

            return Ok(countryDTO);
        }

        //[HttpGet("owners/{ownerId}")]
        //[ProducesResponseType(200, Type = typeof(Country))]
        //[ProducesResponseType(400)]
        //public IActionResult GetCountryOfAnOwner(int ownerId)
        //{
        //    if (!_ownerRepository.OwnerExists(ownerId))
        //        return NotFound();

        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    var country = _mapper
        //        .Map<CountryDTO>(_countryRepositry.GetCountryByOwner(ownerId));

        //    return Ok(country);
        //}

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> CreateCountry([FromBody] CountryDTO countryDTO)
        {
            if (countryDTO is null)
                return BadRequest(ModelState);

            var countries = await _countryRepositry.GetCountries();

            var countryExists = countries
                .Any(x => x.Name.Trim().Equals(countryDTO.Name.Trim(), StringComparison.CurrentCultureIgnoreCase));

            if (countryExists)
            {
                ModelState.AddModelError("", "Country already exists.");
                return StatusCode(422, ModelState);
            }
            var country = countryDTO.MapToEntity();

            if (! await _countryRepositry.CreateCountry(country))
            {
                ModelState.AddModelError("", "Something went wrong while saving.");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpPut("{countryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateCountry(int countryId, [FromQuery] string name)
        {
            if (name.IsNullOrEmpty())
                return BadRequest(ModelState);

            if (! await _countryRepositry.CountryExists(countryId))
                return NotFound(ModelState);

            if (! await _countryRepositry.UpdateCountry(countryId, name))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{countryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteCountry(int countryId)
        {
            if (! await _countryRepositry.CountryExists(countryId))
                return NotFound();

            if (! await _countryRepositry.DeleteCountry(countryId))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
