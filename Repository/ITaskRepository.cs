namespace Repository;

public interface ITaskRepository {
    // Define methods for task-related data operations
    void AddTask(Task task);
    Task GetTaskById(int id);
    IEnumerable<Task> GetAllTasks();
}