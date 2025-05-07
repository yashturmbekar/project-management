using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Services;
using Domain;
using Utilities;
using ProjectManagementAPI.DTO;
using System.Text.RegularExpressions;

namespace ProjectManagementAPI.Controllers
{
    /// <summary>
    /// Handles authentication-related operations.
    /// </summary>
    /// <remarks>
    /// This controller provides endpoints for user login and authentication.
    /// </remarks>
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
        /// Authenticates a user and generates a JWT token.
        /// </summary>
        /// <param name="loginDto">The user login details.</param>
        /// <returns>A JWT token if authentication is successful.</returns>
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