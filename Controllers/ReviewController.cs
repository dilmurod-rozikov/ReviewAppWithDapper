using Microsoft.AspNetCore.Mvc;
using ReviewApp.Interfaces;
using ReviewApp.Models;
using ReviewAppWithDapper.DTOs;

namespace ReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IPokemonRepository _pokemonRepository;

        public ReviewController(IReviewRepository reviewRepository,
            IReviewerRepository reviewerRepository,
            IPokemonRepository pokemonRepository)
        {
            _pokemonRepository = pokemonRepository;
            _reviewerRepository = reviewerRepository;
            _reviewRepository = reviewRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDTO>))]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<ReviewDTO>>> GetReviews()
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var reviews =await _reviewRepository.GetReviews();
            var reviewDTOs = reviews
                .Select(review => new ReviewDTO(review.Title, review.Description, review.Rating, review.Id))
                .ToList();

            return Ok(reviewDTOs);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(ReviewDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ReviewDTO>> GetReview(int id)
        {
            if (!await _reviewRepository.ReviewExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var review = await _reviewRepository.GetReview(id);
            var reviewDTO = new ReviewDTO(review.Title, review.Description, review.Rating, id);

            return Ok(reviewDTO);
        }

        //[HttpGet("pokemon/{pokemonId}")]
        //[ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        //[ProducesResponseType(400)]
        //[ProducesResponseType(404)]
        //public IActionResult GetReviewsOfPokemon(int pokemonId)
        //{
        //    if (!_reviewRepository.ReviewExists(pokemonId))
        //        return NotFound();

        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    var reviews = _mapper
        //        .Map<List<ReviewDTO>>(_reviewRepository.GetReviewsOfAPokemon(pokemonId));

        //    return Ok(reviews);
        //}

        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Review>> CreateReview
            ([FromQuery]int pokemonId, [FromQuery] int reviewerId, [FromBody] ReviewDTO reviewDTO)
        {
            if (reviewDTO is null|| !ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _reviewerRepository.ReviewerExists(reviewerId) ||
                !await _pokemonRepository.PokemonExists(pokemonId))
            {
                return NotFound();
            }

            var review = reviewDTO.MapToEntity();

            review.Pokemon = await  _pokemonRepository.GetPokemon(pokemonId);
            review.Reviewer = await _reviewerRepository.GetReviewer(reviewerId);

            if (! await _reviewRepository.CreateReview(review))
            {
                ModelState.AddModelError("", "Wo, something went wrong while creating.");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpPut("{reviewId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateReview(int reviewId, [FromBody] ReviewDTO reviewDTO)
        {
            if (reviewDTO is null || !ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _reviewRepository.ReviewExists(reviewId))
                return NotFound(ModelState);

            var review = reviewDTO.MapToEntity();
            review.Id = reviewId;
            if (!await _reviewRepository.UpdateReview(review))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{reviewId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteReview(int reviewId)
        {
            if (!await _reviewRepository.ReviewExists(reviewId))
                return NotFound();

            if (!await _reviewRepository.DeleteReview(reviewId))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
