using AccountantHub.Infrastructure.Persistence.Entities;

namespace AccountantHub.Infrastructure.Identity;

public interface IJwtTokenService
{
    string GenerateToken(ApplicationUser user);
}
