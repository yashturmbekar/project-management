using Domain;
using Utilities;
using System.Security.Claims;

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
            new Claim(ClaimTypes.Email, user.Email)
        };

        // Generate JWT token
        return _jwtHelper.GenerateToken(user.Username, claims, 60); // Token valid for 60 minutes
    }
}