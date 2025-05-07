using Domain;
using Microsoft.EntityFrameworkCore;

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

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async System.Threading.Tasks.Task UpdateUserAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}