using Domain;
using Task = Domain.Task;
namespace Repository;

public class TaskRepository : ITaskRepository {
    private readonly AppDbContext _context;

    public TaskRepository(AppDbContext context) {
        _context = context;
    }

    public void AddTask(Domain.Task task) {
        _context.Tasks.Add(task);
        _context.SaveChanges();
    }

    public Task GetTaskById(int id) {
        return _context.Tasks.FirstOrDefault(t => t.Id == id)!;
    }

    public IEnumerable<Task> GetAllTasks() {
        return _context.Tasks.ToList();
    }

    public void UpdateTask(Task task) {
        var existingTask = _context.Tasks.FirstOrDefault(t => t.Id == task.Id);
        if (existingTask != null) {
            existingTask.Name = task.Name;
            existingTask.Description = task.Description;
            existingTask.DueDate = task.DueDate;
            existingTask.Status = task.Status;
            existingTask.AssignedUserId = task.AssignedUserId;
            _context.SaveChanges();
        }
    }
}