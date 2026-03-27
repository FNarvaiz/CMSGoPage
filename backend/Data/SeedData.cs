using backend.Models;
using Microsoft.EntityFrameworkCore;
using BC = BCrypt.Net.BCrypt;

namespace backend.Data;

public static class SeedData
{
    public static async Task InitializeAsync(AppDbContext db)
    {
        if (await db.Users.AnyAsync(u => u.Role == "Admin"))
            return;

        var admin = new User
        {
            Id = Guid.NewGuid(),
            Email = "admin@cmsgopage.com",
            PasswordHash = BC.HashPassword("Admin123!"),
            Name = "Administrador",
            Role = "Admin",
            IsApproved = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        db.Users.Add(admin);
        await db.SaveChangesAsync();
    }
}
