using Domain;

namespace Repository;

public class UserRepository : IUserRepository {
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context) {
        _context = context;
    }

    public void AddUser(User user) {
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public User GetUserById(int id) {
        return _context.Users.FirstOrDefault(u => u.Id == id)!;
    }

    public IEnumerable<User> GetAllUsers() {
        return _context.Users.ToList();
    }
}