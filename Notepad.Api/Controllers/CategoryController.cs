using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notepad.Business.Interfaces;
using Notepad.Database.Custom;
using Notepad.Database.Dto;
using Notepad.Database.Model;

namespace Notepad.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(object))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> Add([FromBody] CategoryDto category)
        {
            try
            {
                await _categoryService.AddCategory(category);
                return Created("", new { message = "Category has been created" });
            }
            catch (ConflictErrorException Ex)
            {
                return StatusCode(StatusCodes.Status409Conflict, Ex.Message);
            }
            catch (Exception Ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Category))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var categories = await _categoryService.GetCategory(id);
                return Ok(categories);
            }
            catch (NotFoundErrorException Ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, Ex.Message);
            }
            catch (Exception Ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Ex.Message);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(object))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryDto category)
        {
            try
            {
                await _categoryService.UpdateCategory(id, category);
                return Ok(new { message = "Category has been updated" });
            }
            catch (NotFoundErrorException Ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, Ex.Message);
            }
            catch (Exception Ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(object))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _categoryService.DeleteCategory(id);
                return Ok(new { message = "Category has been deleted" });
            }
            catch (NotFoundErrorException Ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, Ex.Message);
            }
            catch (Exception Ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Ex.Message);
            }
        }

        [HttpGet("GetAllCategories")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Category>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var categories = await _categoryService.GetAllCategories();
                return Ok(categories);
            }
            catch (Exception Ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Ex.Message);
            }
        }
    }
}
