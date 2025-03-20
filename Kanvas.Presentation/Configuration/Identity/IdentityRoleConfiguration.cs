using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Presentation.Identity;

namespace Presentation.Configuration.Identity;

public class IdentityRoleConfiguration : IEntityTypeConfiguration<AppRole>
{
    public void Configure(EntityTypeBuilder<AppRole> builder)
    {
        builder.HasKey(r => r.Id);
        
        builder.HasIndex(r => r.NormalizedName).HasDatabaseName("RoleNameIndex").IsUnique();
        
        builder.ToTable("AppRoles");
        
        builder.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();
        
        builder.Property(u => u.Name).HasMaxLength(256);
        builder.Property(u => u.NormalizedName).HasMaxLength(256);
        
        builder.HasMany<AppUserRole>().WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();
        
        builder.HasMany<AppRoleClaim>().WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();
        
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