using AccountantHub.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Identity;

namespace AccountantHub.Infrastructure.Persistence;

public static class DemoUserSeeder
{
    public const string Email = "demo@accountanthub.com";
    public const string Password = "DemoPass123!";
    public const string FullName = "Demo Accountant";

    public static async Task SeedAsync(
        UserManager<ApplicationUser> userManager,
        CancellationToken cancellationToken = default)
    {
        if (await userManager.FindByEmailAsync(Email) is not null)
        {
            return;
        }

        var user = new ApplicationUser
        {
            UserName = Email,
            Email = Email,
            FullName = FullName,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, Password);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Failed to seed demo user: {errors}");
        }
    }
}
