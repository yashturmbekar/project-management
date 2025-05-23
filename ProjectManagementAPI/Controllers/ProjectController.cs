using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementAPI.DTO;
using Services;

using System.Security.Claims;

namespace ProjectManagementAPI.Controllers
{
    /// <summary>
    /// Manages project-related operations.
    /// </summary>
    /// <remarks>
    /// This controller provides endpoints for creating, updating, and retrieving projects.
    /// </remarks>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly ProjectService _projectService;
        private readonly UserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectController"/> class.
        /// </summary>
        /// <param name="projectService">The project service.</param>
        /// <param name="userService">The user service.</param>
        public ProjectController(ProjectService projectService, UserService userService)
        {
            _projectService = projectService;
            _userService = userService;
        }

        /// <summary>
        /// Retrieves all projects.
        /// </summary>
        /// <returns>A list of projects.</returns>
        [HttpGet]
        public IActionResult GetProjects()
        {
                var allProjects = _projectService.GetAllProjects();
                return Ok(allProjects);
        }

        /// <summary>
        /// Creates a new project.
        /// </summary>
        /// <param name="projectDto">The project details.</param>
        /// <returns>The created project.</returns>
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult CreateProject([FromBody] ProjectDto projectDto)
        {
            if (projectDto == null)
            {
                return BadRequest("Project data is required.");
            }

            var project = new Project
            {
                Name = projectDto.Name,
                Description = projectDto.Description,
                StartDate = projectDto.StartDate,
                EndDate = projectDto.EndDate,
            };

            _projectService.CreateProject(project);

            var user = _userService.GetUserById(projectDto.UserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.AssignedProjectId = project.Id;
            _userService.UpdateUser(user);

            return CreatedAtAction(nameof(GetProjects), new { id = project.Id }, project);
        }

        /// <summary>
        /// Updates an existing project.
        /// </summary>
        /// <param name="id">The project ID.</param>
        /// <param name="projectDto">The project data transfer object.</param>
        /// <returns>No content if successful.</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult UpdateProject(int id, [FromBody] ProjectDto projectDto)
        {
            if (projectDto == null)
            {
                return BadRequest("Project data is required.");
            }

            var project = _projectService.GetProjectById(id);
            if (project == null)
            {
                return NotFound();
            }

            project.Name = projectDto.Name;
            project.Description = projectDto.Description;
            project.StartDate = projectDto.StartDate;
            project.EndDate = projectDto.EndDate;

            _projectService.UpdateProject(project);

            return NoContent();
        }

        /// <summary>
        /// Deletes a project.
        /// </summary>
        /// <param name="id">The project ID.</param>
        /// <returns>No content if successful.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult DeleteProject(int id)
        {
            var project = _projectService.GetProjectById(id);
            if (project == null)
            {
                return NotFound();
            }

            _projectService.DeleteProject(id);

            return NoContent();
        }
    }
}