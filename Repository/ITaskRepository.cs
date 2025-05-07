namespace Repository;

public interface ITaskRepository {
    // Define methods for task-related data operations
    void AddTask(Domain.Task task);
    Domain.Task GetTaskById(int id);
    IEnumerable<Domain.Task> GetAllTasks();
    void UpdateTask(Domain.Task task);
}