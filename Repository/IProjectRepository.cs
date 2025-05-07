using Domain;

namespace Repository;

public interface IProjectRepository {
    // Define methods for project-related data operations
    void AddProject(Project project);
    Project GetProjectById(int id);
    void UpdateProject(Project project);
    void DeleteProject(int id);
    Project? GetAssignedProjectForUser(int userId);
    IEnumerable<Project> GetAllProjects();
    Task<Project?> GetProjectByIdAsync(int projectId);
    System.Threading.Tasks.Task UpdateProjectAsync(Project project);
}