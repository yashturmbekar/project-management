using Microsoft.AspNetCore.Mvc;
using Services;
using Domain;
using ProjectManagementAPI.DTO;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;

namespace ProjectManagementAPI.Controllers
{
    /// <summary>
    /// Controller for managing user-related operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly ProjectService _projectService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        /// <param name="projectService">The project service.</param>
        public UserController(UserService userService, ProjectService projectService)
        {
            _userService = userService;
            _projectService = projectService;
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

        /// <summary>
        /// Assigns a user to a project.
        /// </summary>
        /// <param name="id">The ID of the user to assign.</param>
        /// <param name="projectId">The ID of the project to assign the user to.</param>
        /// <returns>No content if successful.</returns>
        [HttpPost("{id}/assign-project")]
        [Authorize(Roles = "Admin")]
        public IActionResult AssignUserToProject(int id, [FromBody] int projectId)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var project = _projectService.GetProjectById(projectId);
            if (project == null)
            {
                return NotFound("Project not found.");
            }

            user.AssignedProjectId = projectId;
            if (!_userService.UpdateUser(user))
            {
                return BadRequest("Failed to update user.");
            }

            return NoContent();
        }
    }
}