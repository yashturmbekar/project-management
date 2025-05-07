namespace Repository;

public class TaskRepository : ITaskRepository {
    public void AddTask(Task task) {
        // Implementation for adding a task
    }

    public Task GetTaskById(int id) {
        // Implementation for retrieving a task by ID
        return new Task(); // Return a default Task instance to avoid null reference warnings
    }

    public IEnumerable<Task> GetAllTasks() {
        // Implementation for retrieving all tasks
        return null;
    }
}