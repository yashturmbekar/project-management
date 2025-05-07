using Domain;

namespace Repository;

public class UserRepository : IUserRepository {
    public void AddUser(User user) {
        // Implementation for adding a user
    }

    public User GetUserById(int id) {
        // Implementation for retrieving a user by ID
        return null;
    }

    public IEnumerable<User> GetAllUsers() {
        // Implementation for retrieving all users
        return null;
    }
}