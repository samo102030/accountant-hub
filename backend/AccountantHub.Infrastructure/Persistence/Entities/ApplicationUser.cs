using Microsoft.AspNetCore.Identity;

namespace AccountantHub.Infrastructure.Persistence.Entities;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
}
