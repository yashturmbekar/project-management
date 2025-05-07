using Domain;
using Repository;

namespace Services
{
    public class ProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public Project GetProjectsForUser(string userId)
        {
            if (!int.TryParse(userId, out int userIdInt))
            {
                throw new ArgumentException("Invalid user ID format.", nameof(userId));
            }

            return _projectRepository.GetAssignedProjectForUser(int.Parse(userId));
        }

        public void CreateProject(Project project)
        {
            _projectRepository.AddProject(project);
        }

        public bool ValidateProjectFields(string name, string description, DateTime startDate, DateTime endDate, out string validationMessage)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                validationMessage = "Project name is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                validationMessage = "Project description is required.";
                return false;
            }

            if (startDate >= endDate)
            {
                validationMessage = "Start date must be earlier than end date.";
                return false;
            }

            validationMessage = string.Empty;
            return true;
        }

        public Project GetProjectById(int id)
        {
            return _projectRepository.GetAllProjects().FirstOrDefault(p => p.Id == id) ?? throw new InvalidOperationException("Project not found.");
        }

        public void UpdateProject(Project project)
        {
            _projectRepository.UpdateProject(project);
        }

        public void DeleteProject(int id)
        {
            _projectRepository.DeleteProject(id);
        }

        public IEnumerable<Project> GetAllProjects()
        {
            return _projectRepository.GetAllProjects();
        }

        public Project? GetAssignedProjectForUser(int userId)
        {
            return _projectRepository.GetAssignedProjectForUser(userId);
        }
    }
}