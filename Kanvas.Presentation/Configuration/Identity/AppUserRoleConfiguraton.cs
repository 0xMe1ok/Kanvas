using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Presentation.Identity;

namespace Presentation.Configuration.Identity;

public class AppUserRoleConfiguraton : IEntityTypeConfiguration<AppUserRole>
{
    public void Configure(EntityTypeBuilder<AppUserRole> builder)
    {
        builder.HasKey(r => new { r.UserId, r.RoleId });
        builder.ToTable("UserRoles");
        
        var userRoles = new List<AppUserRole>()
        {
            new()
            {
                RoleId = Guid.Parse("9e4f49fe-0786-44c6-9061-53d2aa84fab1"), // admin
                UserId = Guid.Parse("9e4f49fe-0786-44c6-9061-53d2aa84fab1")
            },
            new()
            {
                RoleId = Guid.Parse("9e4f49fe-0786-44c6-9061-53d2aa84fab2"), // user
                UserId = Guid.Parse("9e4f49fe-0786-44c6-9061-53d2aa84fab2")
            }
        };
        
        builder.HasData(userRoles);
    }
}