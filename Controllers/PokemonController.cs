using Microsoft.AspNetCore.Mvc;
using ReviewApp.Interfaces;
using ReviewAppWithDapper.DTOs;

namespace ReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICategoryRepository _categoryRepository;

        public PokemonController(
            IPokemonRepository pokemonRepository,
            IOwnerRepository ownerRepository,
            ICategoryRepository categoryRepository)
        {
            _pokemonRepository = pokemonRepository;
            _ownerRepository = ownerRepository;
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PokemonDTO>))]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<PokemonDTO>>> GetPokemons()
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var pokemons = await _pokemonRepository.GetPokemons();
            var pokemonDTOs = pokemons.Select(pokemon => new PokemonDTO(pokemon.Name, pokemon.BirthDate)).ToList();

            return Ok(pokemonDTOs);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(PokemonDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PokemonDTO>> GetPokemon(int id)
        {
            if (!await _pokemonRepository.PokemonExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pokemon = await _pokemonRepository.GetPokemon(id);
            var pokemonDTO = new PokemonDTO(pokemon.Name, pokemon.BirthDate);

            return Ok(pokemonDTO);
        }

        [HttpGet("{id}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<decimal>> GetPokemonRating(int id)
        {
            if (!await _pokemonRepository.PokemonExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var rating = await _pokemonRepository.GetPokemonRating(id);

            return Ok(rating);
        }

        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> CreatePokemon
            ([FromQuery] int ownerId, [FromQuery] int categoryId, [FromBody] PokemonDTO pokemonDTO)
        {
            if (pokemonDTO is null || !ModelState.IsValid)
                return BadRequest(ModelState);

            var pokemons = await _pokemonRepository.GetPokemons();
            var pokemonExists = pokemons
                .Any(x => x.Name.Trim().Equals(pokemonDTO.Name.Trim(), StringComparison.OrdinalIgnoreCase));

            if (pokemonExists)
            {
                ModelState.AddModelError("", "Pokemon already exists");
                return StatusCode(422, ModelState);
            }

            if (!await _ownerRepository.OwnerExists(ownerId) ||
                !await _categoryRepository.CategoryExists(categoryId))
            {
                return NotFound(ModelState);
            }
            var pokemon = pokemonDTO.MapToEntity();
            if (! await _pokemonRepository.CreatePokemon(ownerId, categoryId, pokemon))
            {
                ModelState.AddModelError("", "Something went wrong while creating");
            }

            return NoContent();
        }

        [HttpPut("{pokemonId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdatePokemon(int pokemonId, [FromQuery] int ownerId,
            [FromQuery] int categoryId, [FromBody] PokemonDTO pokemonDTO)
        {
            if (pokemonDTO is null || !ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _pokemonRepository.PokemonExists(pokemonId) ||
                !await _ownerRepository.OwnerExists(ownerId) ||
                !await _categoryRepository.CategoryExists(categoryId))
            {
                return NotFound(ModelState);
            }

            var pokemon = pokemonDTO.MapToEntity();

            if (!await _pokemonRepository.UpdatePokemon(ownerId, categoryId, pokemon))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{pokemonId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeletePokemon(int pokemonId)
        {
            if (!await _pokemonRepository.PokemonExists(pokemonId))
                return NotFound();

            if (!await _pokemonRepository.DeletePokemon(pokemonId))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
