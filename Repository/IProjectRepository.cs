using Domain;

namespace Repository;

public interface IProjectRepository {
    // Define methods for project-related data operations
    void AddProject(Project project);
    Project GetProjectById(int id);
    IEnumerable<Project> GetAllProjects();
    void UpdateProject(Project project);
    void DeleteProject(int id);
}