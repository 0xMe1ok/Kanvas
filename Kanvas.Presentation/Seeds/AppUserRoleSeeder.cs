using Microsoft.EntityFrameworkCore;
using Presentation.Identity;

namespace Presentation.Seeds;

public class AppUserRoleSeeder : ISeeder
{
    public static void Seed(DbContext context)
    {
        var adminRole = context.Set<AppUserRole>().FirstOrDefault(u =>
            u.RoleId == Guid.Parse("9e4f49fe-0786-44c6-9061-53d2aa84fab1") &&
            u.UserId == Guid.Parse("9e4f49fe-0786-44c6-9061-53d2aa84fab1"));
        if (adminRole is null)
        {
            adminRole = new()
            {
                RoleId = Guid.Parse("9e4f49fe-0786-44c6-9061-53d2aa84fab1"), // admin
                UserId = Guid.Parse("9e4f49fe-0786-44c6-9061-53d2aa84fab1")
            };
            context.Set<AppUserRole>().Add(adminRole);
        }

        var userRole = context.Set<AppUserRole>().FirstOrDefault(u =>
            u.RoleId == Guid.Parse("9e4f49fe-0786-44c6-9061-53d2aa84fab1") &&
            u.UserId == Guid.Parse("9e4f49fe-0786-44c6-9061-53d2aa84fab2"));
        if (userRole is null)
        {
            userRole = new()
            {
                RoleId = Guid.Parse("9e4f49fe-0786-44c6-9061-53d2aa84fab2"), // user
                UserId = Guid.Parse("9e4f49fe-0786-44c6-9061-53d2aa84fab2")
            };
            context.Set<AppUserRole>().Add(userRole);
        }
        context.SaveChanges();
    }

    public static async Task SeedAsync(DbContext context, CancellationToken ct = default)
    {
        var adminRole = await context.Set<AppUserRole>().FirstOrDefaultAsync(u =>
            u.RoleId == Guid.Parse("9e4f49fe-0786-44c6-9061-53d2aa84fab1") &&
            u.UserId == Guid.Parse("9e4f49fe-0786-44c6-9061-53d2aa84fab1"), cancellationToken: ct);
        if (adminRole is null)
        {
            adminRole = new()
            {
                RoleId = Guid.Parse("9e4f49fe-0786-44c6-9061-53d2aa84fab1"), // admin
                UserId = Guid.Parse("9e4f49fe-0786-44c6-9061-53d2aa84fab1")
            };
            await context.Set<AppUserRole>().AddAsync(adminRole, ct);
        }

        var userRole = await context.Set<AppUserRole>().FirstOrDefaultAsync(u =>
            u.RoleId == Guid.Parse("9e4f49fe-0786-44c6-9061-53d2aa84fab1") &&
            u.UserId == Guid.Parse("9e4f49fe-0786-44c6-9061-53d2aa84fab2"), cancellationToken: ct);
        if (userRole is null)
        {
            userRole = new()
            {
                RoleId = Guid.Parse("9e4f49fe-0786-44c6-9061-53d2aa84fab2"), // user
                UserId = Guid.Parse("9e4f49fe-0786-44c6-9061-53d2aa84fab2")
            };
            await context.Set<AppUserRole>().AddAsync(userRole, ct);
        }
        await context.SaveChangesAsync(ct);
    }
}