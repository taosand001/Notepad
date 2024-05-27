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
    public class NoteController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NoteController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(object))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> Add([FromForm] NoteDto note)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                //var note = new NoteDto(title, content, image, categories);
                var successMessage = await _noteService.Create(note, User.Identity.Name);
                return Created("", new { message = successMessage });
            }
            catch (ConflictErrorException Ex)
            {
                return StatusCode(StatusCodes.Status409Conflict, Ex.Message);
            }
            catch (NotFoundErrorException Ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, Ex.Message);
            }
            catch (Exception Ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Note))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var note = await _noteService.GetById(id);
                return Ok(note);
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
        public async Task<IActionResult> Update(int id, [FromForm] UpdateNoteDto updateNote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {

                var successMessage = await _noteService.Update(id, updateNote, User.Identity.Name);
                return Ok(new { message = successMessage });
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
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var username = User.Identity.Name;
                var successMessage = await _noteService.Delete(id, username);
                return Ok(new { message = successMessage });
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

        [HttpGet("GetAllNotes")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Note>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> GetAllNotes()
        {
            try
            {
                var notes = await _noteService.GetAll();
                return Ok(notes);
            }
            catch (Exception Ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Ex.Message);
            }
        }

        [HttpGet("GetNotesByCategory/{categoryName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Note>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> GetNotesByCategory(string categoryName)
        {
            try
            {
                var notes = await _noteService.GetByCategory(categoryName);
                return Ok(notes);
            }
            catch (Exception Ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Ex.Message);
            }
        }

        [HttpGet("GetNotesByTitle/{title}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Note>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> GetNotesByTitle(string title)
        {
            try
            {
                var notes = await _noteService.GetByTitle(title);
                return Ok(notes);
            }
            catch (Exception Ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Ex.Message);
            }
        }

        [HttpGet("GetUserNotes")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Note>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> GetUserNotes()
        {
            try
            {
                var notes = await _noteService.GetByUser(User.Identity.Name);
                return Ok(notes);
            }
            catch (Exception Ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Ex.Message);
            }
        }
    }
}
