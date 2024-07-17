using Microsoft.AspNetCore.Mvc;
using ReviewApp.Interfaces;
using ReviewAppWithDapper.DTOs;

namespace ReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : Controller
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICountryRepository _countryRepository;

        public OwnerController(
            IOwnerRepository ownerRepository,
            ICountryRepository countryRepository)
        {
            _ownerRepository = ownerRepository;
            _countryRepository = countryRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<OwnerDTO>))]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<OwnerDTO>>> GetOwners()
        {

            var owners = await _ownerRepository.GetOwners();
            var ownerDTOs = owners
                .Select(owner => new OwnerDTO(owner.Name, owner.Gym, owner.Id))
                .ToList();

            return Ok(ownerDTOs);
        }

        [HttpGet("{ownerId}")]
        [ProducesResponseType(200, Type = typeof(OwnerDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<OwnerDTO>> GetOwner(int ownerId)
        {
            if (! await _ownerRepository.OwnerExists(ownerId))
                return NotFound();

            var owner = await _ownerRepository.GetOwner(ownerId);
            var ownerDTO = new OwnerDTO(owner.Name, owner.Gym, owner.Id);

            return Ok(ownerDTO);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> CreateOwner([FromQuery] int countryId, [FromBody] OwnerDTO ownerDTO)
        {
            if (ownerDTO is null)
                return BadRequest(ModelState);

            if (! await _countryRepository.CountryExists(countryId))
                return NotFound(ModelState);

            var owners = await _ownerRepository.GetOwners();
            var ownerExists = owners
                .Any(x => x.Name.Trim().Equals(ownerDTO.Name.Trim(), StringComparison.CurrentCultureIgnoreCase));

            if (ownerExists)
            {
                ModelState.AddModelError("", "Owner already exists.");
                return StatusCode(422, ModelState);
            }

            var owner = ownerDTO.MapToEntity();
            owner.Country = await _countryRepository.GetCountry(countryId);

            if (! await _ownerRepository.CreateOwner(owner))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpPut("{ownerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateOwner(int ownerId, [FromBody] OwnerDTO ownerDTO)
        {
            if (ownerDTO is null)
                return BadRequest(ModelState);

            if (! await _ownerRepository.OwnerExists(ownerId))
                return NotFound(ModelState);

            var owner = ownerDTO.MapToEntity();
            owner.Id = ownerId;
            if (! await _ownerRepository.UpdateOwner(owner))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{ownerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteOwner(int ownerId)
        {
            if (!await _ownerRepository.OwnerExists(ownerId))
                return NotFound();

            if (! await _ownerRepository.DeleteOwner(ownerId))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
