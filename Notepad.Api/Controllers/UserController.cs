using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notepad.Business.Interfaces;
using Notepad.Database.Custom;
using Notepad.Database.Dto;

namespace Notepad.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("loginUser")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> Login([FromBody] LoginDto user)
        {
            try
            {
                var token = await _userService.Login(user);
                return Ok(token);
            }
            catch (UnauthorizedErrorException Ex)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, Ex.Message);
            }
            catch (Exception Ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Ex.Message);
            }
        }

        [HttpPost("registerUser")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(object))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> Register([FromBody] UserDto user)
        {
            try
            {
                var token = await _userService.Register(user);
                return Created("", new { message = "User has been created", token });
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

        [HttpPut("updateRole/{userName}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(object))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> UpdateUserRole(string userName, [FromBody] RoleType role)
        {
            try
            {
                await _userService.UpdateUserRole(userName, role);
                return Ok(new { message = "User role has been updated" });
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

        [HttpDelete("deleteRole/{userName}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(object))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> DeleteUserAdminRole(string userName)
        {
            try
            {
                await _userService.DeleteUserAdminRole(userName);
                return Ok(new { message = "User role has been updated" });
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

        [HttpPut("changePassword")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(object))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePassword)
        {
            try
            {
                await _userService.ChangePassword(User.Identity.Name, changePassword);
                return Ok(new { message = "Password has been changed" });
            }
            catch (UnauthorizedErrorException Ex)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, Ex.Message);
            }
            catch (Exception Ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Ex.Message);
            }
        }

    }
}
