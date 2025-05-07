namespace ProjectManagementAPI.DTO
{
    /// <summary>
    /// Data transfer object for projects.
    /// </summary>
    public class ProjectDto
    {
        /// <summary>
        /// Gets or sets the name of the project.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the project.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the start date of the project.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date of the project.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user associated with the project.
        /// </summary>
        public int UserId { get; set; }
    }
}