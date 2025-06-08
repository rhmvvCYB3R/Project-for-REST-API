using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using REST_project.Controllers.Models;
using REST_project.Controllers.Services.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace REST_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserMockService _userMockService;

        public AuthController(IUserMockService userMockService)
        {
            _userMockService = userMockService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserDTO user)
        {
            if (_userMockService.Register(user))
                return Ok(new {message = "User registered." });
            return BadRequest(new { message = "User already exists." });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserDTO user)
        {
            string? token = _userMockService.Authinticate(user);
            if(token == null)
                return Unauthorized( new { message = "Invalid username or passwaord." });
            return Ok(new { token });
        }

        [HttpGet("secure")]
        [Authorize]
        public IActionResult Secure()
        {
            return Ok(new { message = $"Hi, {_userMockService.getCurrentUser()!.Username}!!! This is secure endpoint." });
        }

        [HttpPut("update")]
        [Authorize]
        public IActionResult UpdateUser([FromBody] UserDTO updatedUser)
        {
            var currentUser = _userMockService.getCurrentUser();
            if (currentUser == null || currentUser.Username != updatedUser.Username)
                return Unauthorized(new { message = "Unauthorized user." });

            bool result = _userMockService.PutUser(updatedUser);
            if (result)
                return Ok(new { message = "Update successful" });
            return NotFound(new { message = "User not found." });
        }

        [HttpDelete("delete")]
        [Authorize]
        public IActionResult DeleteUser()
        {
            var currentUser = _userMockService.getCurrentUser();
            if (currentUser == null)
                return Unauthorized(new { message = "Unauthorized user." });

            bool result = _userMockService.DeleteUser(currentUser.Username);
            if (result)
                return Ok(new { message = "User was deleted." });
            return NotFound(new { message = "User not found." });
        }

    }
}
