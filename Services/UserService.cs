using Domain;
using Utilities;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Services;

public class UserService {
    private readonly AppDbContext _context;
    private readonly JwtHelper _jwtHelper;

    public UserService(AppDbContext context, JwtHelper jwtHelper)
    {
        _context = context;
        _jwtHelper = jwtHelper;
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

    public bool UpgradeUserToManager(int userId)
    {
        try
        {
            // Find the user by ID
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return false; // User not found
            }

            // Update the user's role to Manager
            user.Role = "Manager";

            // Save changes to the database
            _context.SaveChanges();

            return true;
        }
        catch
        {
            return false;
        }
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

    public bool UpdateUserRoleToManager(int userId)
    {
        try
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return false; // User not found
            }

            user.Role = "Manager";
            _context.SaveChanges();

            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool CanAssignToProject(int userId, out string validationMessage)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
        {
            validationMessage = "User not found.";
            return false;
        }

        if (user.AssignedProjectId != null)
        {
            validationMessage = "The user is already assigned to a project.";
            return false;
        }

        validationMessage = string.Empty;
        return true;
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
}