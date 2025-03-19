using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Presentation.Identity;

namespace Presentation.Configuration;

public class IdentityRoleConfiguration : IEntityTypeConfiguration<AppRole>
{
    public void Configure(EntityTypeBuilder<AppRole> builder)
    {
        builder.ToTable("AppRoles");
        var roles = new List<AppRole>
        {
            new()
            {
                Id = Guid.Parse("9e4f49fe-0786-44c6-9061-53d2aa84fab1"),
                Name = "Admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = "9e4f49fe-0786-44c6-9061-53d2aa84fab3"
            },
            new()
            {
                Id = Guid.Parse("9e4f49fe-0786-44c6-9061-53d2aa84fab2"),
                Name = "User",
                NormalizedName = "USER",
                ConcurrencyStamp = "9e4f49fe-0786-44c6-9061-53d2aa84fab3"
            }
        };
        builder.HasData(roles);
    }
}