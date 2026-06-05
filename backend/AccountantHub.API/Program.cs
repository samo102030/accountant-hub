using AccountantHub.Infrastructure;
using AccountantHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = DatabaseConnection.FromEnvironment()
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Database connection string is not configured. Set DATABASE_URL or ConnectionStrings:DefaultConnection.");
}

builder.Services.AddInfrastructure(connectionString);

var corsOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
    ?? ["http://localhost:4200"];

builder.Services.AddCors(options =>
{
    options.AddPolicy("AppCors", policy =>
    {
        policy.SetIsOriginAllowed(origin =>
                corsOrigins.Contains(origin, StringComparer.OrdinalIgnoreCase) ||
                origin.StartsWith("http://localhost:", StringComparison.OrdinalIgnoreCase) ||
                origin.EndsWith(".netlify.app", StringComparison.OrdinalIgnoreCase))
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    await DbSeeder.SeedAsync(db);
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AppCors");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
