namespace ProjectManagementAPI.DTO
{
    /// <summary>
    /// Data transfer object for tasks.
    /// </summary>
    public class TaskDto
    {
        /// <summary>
        /// Gets or sets the name of the task.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the task.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the due date of the task.
        /// </summary>
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Gets or sets the ID of the project the task belongs to.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user assigned to the task.
        /// </summary>
        public int AssignedUserId { get; set; }
    }
}