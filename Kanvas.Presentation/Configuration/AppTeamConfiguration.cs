using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Presentation.Entities;

namespace Presentation.Configuration;

public class AppTeamConfiguration : IEntityTypeConfiguration<AppTeam>
{
    public void Configure(EntityTypeBuilder<AppTeam> builder)
    {
        builder.ToTable("Teams");
        builder.HasKey(team => team.Id);
        builder.Property(team => team.Id).ValueGeneratedOnAdd();
        builder.Property(team => team.OwnerId).HasDefaultValue(Guid.NewGuid()); // user here
        builder
            .HasMany(team => team.Boards)
            .WithOne(board => board.Team)
            .HasForeignKey(t => t.TeamId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}