using Domain;
using Repository;
using Microsoft.Extensions.Logging;
using Task = Domain.Task;

namespace Services
{
    public class TaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger<TaskService> _logger;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;

        public TaskService(ITaskRepository taskRepository, ILogger<TaskService> logger, IProjectRepository projectRepository, IUserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _logger = logger;
            _projectRepository = projectRepository;
            _userRepository = userRepository;
        }

        public IEnumerable<Task> GetTasksForUser(string userId)
        {
            _logger.LogInformation("Fetching tasks for user with ID: {UserId}", userId);
            int userIdInt = int.Parse(userId);
            var tasks = _taskRepository.GetAllTasks().Where(t => t.AssignedUserId == userIdInt);
            _logger.LogInformation("Found {TaskCount} tasks for user with ID: {UserId}", tasks.Count(), userId);
            return tasks;
        }

        public void CreateTask(Task task)
        {
            _logger.LogInformation("Creating a new task: {TaskName}", task.Name);
            _taskRepository.AddTask(task);
            _logger.LogInformation("Task created successfully: {TaskName}", task.Name);
        }

        public bool ValidateTaskFields(string title, string description, DateTime dueDate, string status, int assignedToUserId, out string validationMessage)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                validationMessage = "Task title is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                validationMessage = "Task description is required.";
                return false;
            }

            if (dueDate <= DateTime.Now)
            {
                validationMessage = "Due date must be in the future.";
                return false;
            }

            var validStatuses = new[] { "Pending", "In Progress", "Completed" };
            if (!validStatuses.Contains(status))
            {
                validationMessage = "Invalid task status. Valid statuses are: Pending, In Progress, Completed.";
                return false;
            }

            if (assignedToUserId <= 0)
            {
                validationMessage = "Assigned user ID must be a positive integer.";
                return false;
            }

            validationMessage = string.Empty;
            return true;
        }

        public bool CanAssignTask(int assignedToUserId, out string validationMessage)
        {
            var existingTask = _taskRepository.GetAllTasks().FirstOrDefault(t => t.AssignedUserId == assignedToUserId && t.Status != Domain.TaskStatus.Completed);
            if (existingTask != null)
            {
                validationMessage = "The employee is already assigned to an active task.";
                return false;
            }

            validationMessage = string.Empty;
            return true;
        }

        public Task GetTaskById(int id)
        {
            _logger.LogInformation("Fetching task with ID: {TaskId}", id);
            return _taskRepository.GetTaskById(id);
        }

        public void UpdateTask(Task task)
        {
            _logger.LogInformation("Updating task with ID: {TaskId}", task.Id);
            _taskRepository.UpdateTask(task);
            _logger.LogInformation("Task updated successfully with ID: {TaskId}", task.Id);
        }

        public bool ValidateProjectAndUserExistence(int projectId, int assignedToUserId, out string validationMessage)
        {
            var projectExists = _projectRepository.GetProjectById(projectId) != null;
            if (!projectExists)
            {
                validationMessage = "The specified project does not exist.";
                return false;
            }

            var userExists = _userRepository.GetUserById(assignedToUserId) != null;
            if (!userExists)
            {
                validationMessage = "The specified user does not exist.";
                return false;
            }

            validationMessage = string.Empty;
            return true;
        }
    }
}