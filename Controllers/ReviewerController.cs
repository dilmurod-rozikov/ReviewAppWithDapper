using Microsoft.AspNetCore.Mvc;
using ReviewApp.Interfaces;
using ReviewApp.Models;
using ReviewAppWithDapper.DTOs;

namespace ReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController : Controller
    {
        private readonly IReviewerRepository _reviewerRepository;

        public ReviewerController(IReviewerRepository reviewerRepository)
        {
            _reviewerRepository = reviewerRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewerDTO>))]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<ReviewerDTO>>> GetReviewers()
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var reviewers = await _reviewerRepository.GetReviewers();
            var reviewerDTOs = reviewers
                .Select(reviewer => new ReviewerDTO(reviewer.FirstName, reviewer.LastName, reviewer.Id))
                .ToList();

            return Ok(reviewerDTOs);
        }

        [HttpGet("{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(ReviewerDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ReviewerDTO>> GetReviewer(int reviewerId)
        {
            if (! await _reviewerRepository.ReviewerExists(reviewerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewer = await _reviewerRepository.GetReviewer(reviewerId);
            var reviewerDTO = new ReviewerDTO(reviewer.FirstName, reviewer.LastName, reviewer.Id);
            return Ok(reviewerDTO);
        }

        //[HttpGet("{reviewerId}/reviews")]
        //[ProducesResponseType(200, Type = typeof(Review))]
        //[ProducesResponseType(400)]
        //[ProducesResponseType(404)]
        //public IActionResult GetReviewsOfReviewer(int reviewerId)
        //{
        //    if (!_reviewerRepository.ReviewerExists(reviewerId))
        //        return NotFound();

        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    var reviews = _mapper
        //        .Map<List<ReviewDTO>>(_reviewerRepository.GetReviewsByReviewer(reviewerId));

        //    return Ok(reviews);
        //}

        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Reviewer>> CreateReviewer([FromBody] ReviewerDTO reviewerDTO)
        {
            if (reviewerDTO is null || !ModelState.IsValid)
                return BadRequest(ModelState);

            var trimmedReviewerName = reviewerDTO.LastName.Trim() + reviewerDTO.FirstName.Trim();
            var reviewers = await _reviewerRepository.GetReviewers();
            var reviewerExists = reviewers
                .Any(x => (x.LastName.Trim() + x.FirstName.Trim())
                .Equals(trimmedReviewerName, StringComparison.OrdinalIgnoreCase));

            if (reviewerExists)
            {
                ModelState.AddModelError("", "Reviewer already exists.");
                return StatusCode(422, ModelState);
            }

            var reviewer = reviewerDTO.MapToEntity();
            if (!await _reviewerRepository.CreateReviewer(reviewer))
            {
                ModelState.AddModelError("", "Something wen wrong, while creating");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpPut("{reviewerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateReviewer(int reviewerId, [FromBody] ReviewerDTO reviewerDTO)
        {
            if (reviewerDTO is null)
                return BadRequest(ModelState);

            if (! await _reviewerRepository.ReviewerExists(reviewerId))
                return NotFound(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewer = reviewerDTO.MapToEntity();
            reviewer.Id = reviewerId;

            if (! await _reviewerRepository.UpdateReviewer(reviewer))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{reviewerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteReviewer(int reviewerId)
        {
            if (!await _reviewerRepository.ReviewerExists(reviewerId))
                return NotFound();

            if (! await _reviewerRepository.DeleteReviewer(reviewerId))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
