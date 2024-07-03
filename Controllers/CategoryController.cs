using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ReviewApp.Interfaces;
using ReviewApp.Models;
using ReviewAppWithDapper.DTOs;

namespace ReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepositry;
        public CategoryController(ICategoryRepository categoryRepositry)
        {
            _categoryRepositry = categoryRepositry;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDTO>))]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var categories = await _categoryRepositry.GetCategories();
            var categoryDTOs = categories.Select(category => new CategoryDTO(category.Name, category.Id)).ToList();
            return Ok(categoryDTOs);
        }

        [HttpGet("{categoryId}")]
        [ProducesResponseType(200, Type = typeof(CategoryDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<CategoryDTO>> GetCategory(int categoryId)
        {
            if (!await _categoryRepositry.CategoryExists(categoryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = await _categoryRepositry.GetCategory(categoryId);
            var categoryDTO = new CategoryDTO(category.Name, category.Id);

            return Ok(categoryDTO);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> CreateCategory([FromBody] CategoryDTO categoryDTO)
        {
            if (categoryDTO is null || !ModelState.IsValid)
                return BadRequest(ModelState);

            var categories = await _categoryRepositry.GetCategories();
            var categoryNameExists = categories
                .Any(x => x.Name.Trim().Equals(categoryDTO.Name.Trim(), StringComparison.CurrentCultureIgnoreCase));

            if (categoryNameExists)
            {
                ModelState.AddModelError("", "Category name already exists.");
                return StatusCode(422, ModelState);
            }

            var category = categoryDTO.MapToEntity();
            if (!await _categoryRepositry.CreateCategory(category))
            {
                ModelState.AddModelError("", "Something went wrong while saving.");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpPut("{categoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCategory(int categoryId, [FromQuery] string name)
        {
            if (name.IsNullOrEmpty() || !ModelState.IsValid)
                return BadRequest(ModelState);

            if (! await _categoryRepositry.CategoryExists(categoryId))
                return NotFound(ModelState);

            if(! await _categoryRepositry.UpdateCategory(name, categoryId))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{categoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            if (! await _categoryRepositry.CategoryExists(categoryId))
                return NotFound();

            if (! await _categoryRepositry.DeleteCategory(categoryId))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
