using Domain;

namespace Repository;

public class UserRepository : IUserRepository {
    public void AddUser(User user) {
        // Implementation for adding a user
    }

    public User GetUserById(int id) {
        // Implementation for retrieving a user by ID
        return new User(); // Return a default User instance to avoid null reference warnings
    }

    public IEnumerable<User> GetAllUsers() {
        // Implementation for retrieving all users
        return null;
    }
}