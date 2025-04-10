using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Presentation.Entities;
using Presentation.Identity;

namespace Presentation.Configuration.Identity;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.HasKey(u => u.Id);
        
        builder.HasIndex(u => u.NormalizedUserName).HasDatabaseName("UserNameIndex").IsUnique();
        builder.HasIndex(u => u.NormalizedEmail).HasDatabaseName("EmailIndex");
        
        builder.ToTable("Users");
        
        builder.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();
        
        builder.Property(u => u.UserName).HasMaxLength(256);
        builder.Property(u => u.NormalizedUserName).HasMaxLength(256);
        builder.Property(u => u.Email).HasMaxLength(256);
        builder.Property(u => u.NormalizedEmail).HasMaxLength(256);
        
        builder.HasMany<AppUserClaim>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
        builder.HasMany<AppUserLogin>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();
        builder.HasMany<AppUserToken>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();
        builder.HasMany<AppUserRole>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
            
        builder.HasMany<AppTeam>(user => user.Teams)
            .WithOne()
            .HasForeignKey(team => team.OwnerId)
            .HasPrincipalKey(team => team.Id)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany<AppTask>(user => user.Tasks)
            .WithOne()
            .HasForeignKey(task => task.CreatedBy)
            .HasPrincipalKey(team => team.Id)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}