using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using REST_project.Controllers.Models;
using REST_project.Controllers.Services.Interface;
using System;

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
            try
            {
                if (_userMockService.Register(user))
                    return Ok(new { message = "User registered." });

                return BadRequest(new { message = "User already exists." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error during registration", error = ex.Message });
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserDTO user)
        {
            try
            {
                if (user == null || string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
                    return BadRequest(new { message = "Invalid request: missing username or password." });

                string? token = _userMockService.Authinticate(user);

                if (token == null)
                    return Unauthorized(new { message = "Invalid username or password." });

                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error during login", error = ex.Message });
            }
        }

        [HttpGet("secure")]
        [Authorize]
        public IActionResult Secure()
        {
            try
            {
                var user = _userMockService.getCurrentUser();
                if (user == null)
                    return Unauthorized(new { message = "Unauthorized access." });

                return Ok(new { message = $"Hi, {user.Username}!!! This is a secure endpoint." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error on secure endpoint", error = ex.Message });
            }
        }

        [HttpPut("update")]
        [Authorize]
        public IActionResult UpdateUser([FromBody] UserDTO updatedUser)
        {
            try
            {
                var currentUser = _userMockService.getCurrentUser();
                if (currentUser == null || currentUser.Username != updatedUser.Username)
                    return Unauthorized(new { message = "Unauthorized user." });

                bool result = _userMockService.PutUser(updatedUser);
                if (result)
                    return Ok(new { message = "Update successful" });

                return NotFound(new { message = "User not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error during update", error = ex.Message });
            }
        }

        [HttpDelete("delete")]
        [Authorize]
        public IActionResult DeleteUser()
        {
            try
            {
                var currentUser = _userMockService.getCurrentUser();
                if (currentUser == null)
                    return Unauthorized(new { message = "Unauthorized user." });

                bool result = _userMockService.DeleteUser(currentUser.Username);
                if (result)
                    return Ok(new { message = "User was deleted." });

                return NotFound(new { message = "User not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error during delete", error = ex.Message });
            }
        }
    }
}
