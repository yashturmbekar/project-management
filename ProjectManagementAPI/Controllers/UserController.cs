using Microsoft.AspNetCore.Mvc;
using Services;
using Domain;
using ProjectManagementAPI.DTO;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;

namespace ProjectManagementAPI.Controllers
{
    /// <summary>
    /// Manages user-related operations.
    /// </summary>
    /// <remarks>
    /// This controller provides endpoints for creating, updating, and retrieving users.
    /// </remarks>
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

        /// <summary>
        /// Upgrades a user's role to Manager.
        /// </summary>
        /// <param name="userId">The ID of the user to upgrade.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPatch("{userId}/upgrade-to-manager")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpgradeToManager(int userId)
        {
            var result = await _userService.UpgradeUserToManagerAsync(userId);
            if (!result)
            {
                return NotFound("User not found or failed to upgrade role.");
            }

            return Ok("User upgraded to Manager successfully.");
        }

        /// <summary>
        /// Assigns a project to an employee.
        /// </summary>
        /// <param name="userId">The ID of the user to assign.</param>
        /// <param name="projectId">The ID of the project to assign the user to.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPatch("{userId}/assign-project")]
        [Authorize(Roles = "Manager")]
        public IActionResult AssignProjectToUser(int userId, [FromBody] int projectId)
        {
            var user = _userService.GetUserById(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (user.Role != "Employee")
            {
                return BadRequest("Only employees can be assigned to projects.");
            }

            var project = _projectService.GetProjectById(projectId);
            if (project == null)
            {
                return NotFound("Project not found.");
            }

            user.AssignedProjectId = projectId;
            _userService.UpdateUser(user);

            return Ok("Project assigned successfully.");
        }
    }
}