using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Services;
using Domain;
using Utilities;

namespace ProjectManagementAPI.Controllers
{
    /// <summary>
    /// Controller for handling authentication-related operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly JwtHelper _jwtHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        /// <param name="jwtHelper">The JWT helper.</param>
        public AuthController(UserService userService, JwtHelper jwtHelper)
        {
            _userService = userService;
            _jwtHelper = jwtHelper;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="user">The user to register.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            var result = _userService.RegisterUser(user);
            if (!result)
            {
                return BadRequest("User registration failed.");
            }
            return Ok("User registered successfully.");
        }

        /// <summary>
        /// Logs in a user.
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

    /// <summary>
    /// DTO for user login.
    /// </summary>
    public class UserLoginDto
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}