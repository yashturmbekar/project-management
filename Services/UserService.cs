using Domain;
using Utilities;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace Services;

public class UserService {
    private readonly AppDbContext _context;
    private readonly JwtHelper _jwtHelper;
    private readonly IUserRepository _userRepository;
    private readonly IProjectRepository _projectRepository;

    public UserService(AppDbContext context, JwtHelper jwtHelper, IUserRepository userRepository, IProjectRepository projectRepository)
    {
        _context = context;
        _jwtHelper = jwtHelper;
        _userRepository = userRepository;
        _projectRepository = projectRepository;
    }
    public bool RegisterUser(User user)
    {
        try
        {
            // Hash the password using BCrypt
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            // Save the user to the database
            _context.Users.Add(user);
            _context.SaveChanges();

            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool RegisterUserWithDefaultRole(User user)
    {
        try
        {
            // Assign default role as Employee
            user.Role = "Employee";

            // Hash the password using BCrypt
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            // Save the user to the database
            _context.Users.Add(user);
            _context.SaveChanges();

            return true;
        }
        catch
        {
            return false;
        }
    }

    public string? LoginUser(string username, string password)
    {
        var user = _context.Users.FirstOrDefault(u => u.Username == username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            return null; // Invalid credentials
        }

        // Create claims for the JWT token
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
        };

        // Generate JWT token
        return _jwtHelper.GenerateToken(user.Role, user.Id, claims, 60); // Token valid for 60 minutes
    }

    public async Task<bool> UpgradeUserToManagerAsync(int userId)
    {
        try
        {
            var rowsAffected = await _context.Database.ExecuteSqlRawAsync(
                "UPDATE \"Users\" SET \"Role\" = {0} WHERE \"Id\" = {1}", "Manager", userId);

            return rowsAffected > 0;
        }
        catch
        {
            return false;
        }
    }

    public User? GetUserById(int userId)
    {
        return _context.Users.FirstOrDefault(u => u.Id == userId);
    }

    public bool UpdateUser(User user)
    {
        try
        {
            _context.Users.Update(user);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> AssignUserToProjectAsync(int userId, int projectId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null)
        {
            throw new ArgumentException("User not found.", nameof(userId));
        }

        if (user.Role != "Employee")
        {
            throw new InvalidOperationException("Only employees can be assigned to projects.");
        }

        if (user.AssignedProjectId != null)
        {
            throw new InvalidOperationException("The employee is already assigned to another project.");
        }

        var project = await _projectRepository.GetProjectByIdAsync(projectId);
        if (project == null)
        {
            throw new ArgumentException("Project not found.", nameof(projectId));
        }

        user.AssignedProjectId = projectId;
        await _userRepository.UpdateUserAsync(user);

        return true;
    }
}