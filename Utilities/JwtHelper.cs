namespace Utilities;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public class JwtHelper {
    private readonly string _secretKey = "YourSuperSecureSecretKey1234567890123456"; // Ensure key is at least 128 bits

    public JwtHelper(string secretKey) {
        if (string.IsNullOrWhiteSpace(secretKey) || secretKey.Length < 32)
        {
            throw new ArgumentException("The secret key must be at least 256 bits (32 characters) long.", nameof(secretKey));
        }

        _secretKey = secretKey;
    }

    public string GenerateToken(string role, int userId, IEnumerable<Claim> claims, int expiryMinutes) {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        claims = claims.Append(new Claim(ClaimTypes.Role, role))
                       .Append(new Claim(ClaimTypes.NameIdentifier, userId.ToString())); // Add NameIdentifier claim

        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}