using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Presentation.Identity;

namespace Presentation.Seeds;

public class AppUserSeeder : ISeeder
{
    public static void Seed(DbContext context)
    {
        var passwordHasher = new PasswordHasher<AppUser>();
        var admin = context.Set<AppUser>().FirstOrDefault(u => u.UserName == "admin");
        if (admin == null)
        {
            admin = new AppUser()
            {
                Id = Guid.Parse("9e4f49fe-0786-44c6-9061-53d2aa84fab1"),
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                EmailConfirmed = true,
            };
            var adminPassword = "@dminskiiBlinskii0";
            admin.PasswordHash = passwordHasher.HashPassword(admin, adminPassword);
            context.Set<AppUser>().Add(admin);
        }
        var user = context.Set<AppUser>().FirstOrDefault(u => u.UserName == "user");
        if (user == null)
        {
            user = new AppUser()
            {
                Id = Guid.Parse("9e4f49fe-0786-44c6-9061-53d2aa84fab2"),
                UserName = "user",
                NormalizedUserName = "USER",
                Email = "user@user.com",
                NormalizedEmail = "USER@USER.COM",
                EmailConfirmed = true,
            };
            var userPassword = "UserIsN@tDefin3d";
            user.PasswordHash = passwordHasher.HashPassword(user, userPassword);
            context.Set<AppUser>().Add(user);
        }
        context.SaveChanges();
    }

    public static async Task SeedAsync(DbContext context, CancellationToken ct = default)
    {
        var passwordHasher = new PasswordHasher<AppUser>();
        var admin = await context.Set<AppUser>().FirstOrDefaultAsync(u => u.UserName == "admin", ct);
        if (admin == null)
        {
            admin = new AppUser()
            {
                Id = Guid.Parse("9e4f49fe-0786-44c6-9061-53d2aa84fab1"),
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                EmailConfirmed = true,
            };
            var adminPassword = "@dminskiiBlinskii0";
            admin.PasswordHash = passwordHasher.HashPassword(admin, adminPassword);
            await context.Set<AppUser>().AddAsync(admin, ct);
        }
        var user = await context.Set<AppUser>().FirstOrDefaultAsync(u => u.UserName == "user", ct);
        if (user == null)
        {
            user = new AppUser()
            {
                Id = Guid.Parse("9e4f49fe-0786-44c6-9061-53d2aa84fab2"),
                UserName = "user",
                NormalizedUserName = "USER",
                Email = "user@user.com",
                NormalizedEmail = "USER@USER.COM",
                EmailConfirmed = true,
            };
            var userPassword = "UserIsN@tDefin3d";
            user.PasswordHash = passwordHasher.HashPassword(user, userPassword);
            await context.Set<AppUser>().AddAsync(user, ct);
        }
        await context.SaveChangesAsync(ct);
    }
}