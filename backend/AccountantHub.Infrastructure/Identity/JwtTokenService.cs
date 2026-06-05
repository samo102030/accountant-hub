using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AccountantHub.Infrastructure.Persistence.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AccountantHub.Infrastructure.Identity;

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(ApplicationUser user)
    {
        var key = GetSigningKey();
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(JwtRegisteredClaimNames.Name, user.FullName),
            new(ClaimTypes.NameIdentifier, user.Id)
        };

        var token = new JwtSecurityToken(
            issuer: GetIssuer(),
            audience: GetAudience(),
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private SymmetricSecurityKey GetSigningKey()
    {
        var secret = Environment.GetEnvironmentVariable("JWT_SECRET")
            ?? _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException(
                "JWT secret is not configured. Set JWT_SECRET or Jwt:Key.");

        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
    }

    private string GetIssuer() =>
        _configuration["Jwt:Issuer"] ?? "AccountantHub";

    private string GetAudience() =>
        _configuration["Jwt:Audience"] ?? "AccountantHub";
}
