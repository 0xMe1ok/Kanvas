using Microsoft.EntityFrameworkCore;
using Presentation.Identity;

namespace Presentation.Seeds;

public class IdentityRoleSeeder : ISeeder
{
    public static void Seed(DbContext context)
    {
        var adminRole = context.Set<AppRole>().FirstOrDefault(u =>
            u.Name == "Admin");
        if (adminRole == null)
        {
            adminRole = new()
            {
                Id = Guid.Parse("9e4f49fe-0786-44c6-9061-53d2aa84fab1"),
                Name = "Admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = "9e4f49fe-0786-44c6-9061-53d2aa84fab3"
            };
            context.Set<AppRole>().Add(adminRole);
        }
        var userRole = context.Set<AppRole>().FirstOrDefault(u => 
            u.Name == "User");
        if (userRole == null)
        {
            userRole = new()
            {
                Id = Guid.Parse("9e4f49fe-0786-44c6-9061-53d2aa84fab2"),
                Name = "User",
                NormalizedName = "USER",
                ConcurrencyStamp = "9e4f49fe-0786-44c6-9061-53d2aa84fab3"
            };
            context.Set<AppRole>().Add(userRole);
        }
        context.SaveChanges();
    }

    public static async Task SeedAsync(DbContext context, CancellationToken ct = default)
    {
        var adminRole = await context.Set<AppRole>().FirstOrDefaultAsync(u =>
            u.Name == "Admin", cancellationToken: ct);
        if (adminRole == null)
        {
            adminRole = new()
            {
                Id = Guid.Parse("9e4f49fe-0786-44c6-9061-53d2aa84fab1"),
                Name = "Admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = "9e4f49fe-0786-44c6-9061-53d2aa84fab3"
            };
            await context.Set<AppRole>().AddAsync(adminRole, ct);
        }
        var userRole = context.Set<AppRole>().FirstOrDefault(u => 
            u.Name == "User");
        if (userRole == null)
        {
            userRole = new()
            {
                Id = Guid.Parse("9e4f49fe-0786-44c6-9061-53d2aa84fab2"),
                Name = "User",
                NormalizedName = "USER",
                ConcurrencyStamp = "9e4f49fe-0786-44c6-9061-53d2aa84fab3"
            };
            await context.Set<AppRole>().AddAsync(userRole, ct);
        }
        await context.SaveChangesAsync(ct);
    }
}