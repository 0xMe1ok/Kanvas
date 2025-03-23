using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Presentation.Entities;
using Presentation.Identity;

namespace Presentation.Configuration;
public class TeamMemberConfiguration : IEntityTypeConfiguration<TeamMember>
{
    public void Configure(EntityTypeBuilder<TeamMember> builder)
    {
        builder.ToTable("TeamMembers");
        builder.HasKey(p => new { p.MemberId, p.TeamId });
        
        builder.HasOne<AppUser>()
            .WithMany(user => user.Memberships)
            .HasForeignKey(tm => tm.MemberId);
            
        builder.HasOne<AppTeam>()
            .WithMany(team => team.Members)
            .HasForeignKey(tm => tm.TeamId);
        
        builder
            .Property(t => t.Role)
            .HasConversion<string>();
    }
}