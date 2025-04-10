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
    }
}