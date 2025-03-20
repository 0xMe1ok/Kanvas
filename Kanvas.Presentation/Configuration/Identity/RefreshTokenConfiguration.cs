using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Presentation.Identity.Tokens;

namespace Presentation.Configuration.Identity;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshToken");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Token).HasMaxLength(200);
        builder.HasIndex(r => r.Token).IsUnique();
        builder
            .HasOne(r => r.AppUser)
            .WithMany()
            .HasForeignKey(r => r.UserId);
    }
}