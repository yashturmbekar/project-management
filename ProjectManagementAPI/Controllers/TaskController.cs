using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementAPI.DTO;
using Services;
using System.Security.Claims;
using Task = Domain.Task;

namespace ProjectManagementAPI.Controllers
{
    /// <summary>
    /// Manages task-related operations.
    /// </summary>
    /// <remarks>
    /// This controller provides endpoints for creating, updating, and retrieving tasks.
    /// </remarks>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly TaskService _taskService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskController"/> class.
        /// </summary>
        /// <param name="taskService">The task service.</param>
        public TaskController(TaskService taskService)
        {
            _taskService = taskService;
        }

        /// <summary>
        /// Retrieves all tasks.
        /// </summary>
        /// <returns>A list of tasks.</returns>
        [HttpGet]
        public IActionResult GetTasksForCurrentUser()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            var tasks = _taskService.GetTasksForUser(userId);
            return Ok(tasks);
        }

        /// <summary>
        /// Creates a new task.
        /// </summary>
        /// <param name="taskDto">The task details.</param>
        /// <returns>The created task.</returns>
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult CreateTask([FromBody] TaskDto taskDto)
        {
            if (taskDto == null)
            {
                return BadRequest("Task data is required.");
            }

            var task = new Task
            {
                Name = taskDto.Name,
                Description = taskDto.Description,
                DueDate = taskDto.DueDate,
                ProjectId = taskDto.ProjectId,
                AssignedUserId = taskDto.AssignedUserId
            };

            _taskService.CreateTask(task);

            return CreatedAtAction(nameof(GetTasksForCurrentUser), new { id = task.Id }, task);
        }

        /// <summary>
        /// Updates the status of a task. Accessible to employees.
        /// </summary>
        /// <param name="id">The ID of the task to update.</param>
        /// <param name="status">The new status of the task.</param>
        /// <returns>No content if successful.</returns>
        [HttpPatch("{id}")]
        [Authorize(Roles = "Employee")]
        public IActionResult UpdateTaskStatus(int id, [FromBody] string status)
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                return BadRequest("Status is required.");
            }

            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            var task = _taskService.GetTaskById(id);
            if (task == null)
            {
                return NotFound();
            }

            if (task.AssignedUserId.ToString() != userId)
            {
                return Forbid("You are not authorized to update this task.");
            }

            if (!Enum.TryParse<TaskStatus>(status, true, out var taskStatus))
            {
                return BadRequest("Invalid task status. Valid statuses are: NotStarted, InProgress, Completed.");
            }

            task.Status = (Domain.TaskStatus)taskStatus;
            _taskService.UpdateTask(task);

            return NoContent();
        }
    }
}