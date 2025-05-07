using Domain;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class ProjectRepository : IProjectRepository {
    private readonly AppDbContext _context;

    public ProjectRepository(AppDbContext context) {
        _context = context;
    }

    public void AddProject(Project project) {
        _context.Projects.Add(project);
        _context.SaveChanges();
    }

    public Project GetProjectById(int id) {
        return _context.Projects.FirstOrDefault(p => p.Id == id) ?? new Project(); // Return a default Project instance to avoid null reference warnings
    }

    public async Task<Project?> GetProjectByIdAsync(int projectId)
    {
        return await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
    }

    public IEnumerable<Project> GetAllProjects() {
        return _context.Projects.ToList();
    }

    public void UpdateProject(Project project) {
        _context.Projects.Update(project);
        _context.SaveChanges();
    }

    public async System.Threading.Tasks.Task UpdateProjectAsync(Project project)
    {
        _context.Projects.Update(project);
        await _context.SaveChangesAsync();
    }

    public void DeleteProject(int id) {
        var project = _context.Projects.FirstOrDefault(p => p.Id == id);
        if (project != null) {
            _context.Projects.Remove(project);
            _context.SaveChanges();
        }
    }

    public Project? GetAssignedProjectForUser(int userId)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null || user.AssignedProjectId == null)
        {
            return null; // Return null if no assigned project
        }

        return _context.Projects.FirstOrDefault(p => p.Id == user.AssignedProjectId);
    }
}