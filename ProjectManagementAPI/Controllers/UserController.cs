using Microsoft.AspNetCore.Mvc;
using Services;
using Domain;
using ProjectManagementAPI.DTO;
using System.Text.RegularExpressions;

namespace ProjectManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    /// <summary>
    /// Controller for managing user-related operations.
    /// </summary>
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Registers a new user with the default role of Employee.
        /// </summary>
        /// <param name="user">The user details for registration.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            // Validate email format
            if (!Regex.IsMatch(user.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                return BadRequest("Invalid email format.");
            }
            // Validate password complexity
            if (!Regex.IsMatch(user.Password, @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$"))
            {
                return BadRequest("Password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, one number, and one special character.");
            }

            var result = _userService.RegisterUserWithDefaultRole(user);
            if (!result)
            {
                return BadRequest("User registration failed.");
            }
            return Ok("User registered successfully.");
        }

        /// <summary>
        /// Logs in a user and returns a JWT token if successful.
        /// </summary>
        /// <param name="loginDto">The login details.</param>
        /// <returns>An IActionResult containing the JWT token if successful.</returns>
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto loginDto)
        {
            var token = _userService.LoginUser(loginDto.Username, loginDto.Password);
            if (token == null)
            {
                return Unauthorized("Invalid username or password.");
            }
            return Ok(new { Token = token });
        }
    }
}