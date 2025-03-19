using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Presentation.Entities;
using Presentation.Identity;

namespace Presentation.Configuration;

public class AppTeamConfiguration : IEntityTypeConfiguration<AppTeam>
{
    public void Configure(EntityTypeBuilder<AppTeam> builder)
    {
        builder.ToTable("Teams");
        builder.HasKey(team => team.Id);
        builder.Property(team => team.Id).ValueGeneratedOnAdd();
    }
}