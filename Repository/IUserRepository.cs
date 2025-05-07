using Domain;

namespace Repository;

public interface IUserRepository {
    // Define methods for user-related data operations
    void AddUser(User user);
    User GetUserById(int id);
    IEnumerable<User> GetAllUsers();
}