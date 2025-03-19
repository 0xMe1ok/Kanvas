using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Presentation.Entities;
using Presentation.Identity;

namespace Presentation.Configuration;

public class AppTaskConfiguration : IEntityTypeConfiguration<AppTask>
{
    public void Configure(EntityTypeBuilder<AppTask> builder)
    {
        builder.ToTable("Tasks");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).ValueGeneratedOnAdd();
        builder
            .HasOne(t => t.Board)
            .WithMany(b => b.Tasks)
            .HasForeignKey(t => t.BoardId);
        builder
            .HasOne(t => t.Column)
            .WithMany(c => c.Tasks)
            .HasForeignKey(t => t.ColumnId);
        builder.HasOne(t => t.Team)
            .WithMany(t => t.Tasks)
            .HasForeignKey(t => t.TeamId);
        builder
            .Property(t => t.Status)
            .HasConversion<string>();
        
        /*
        builder
            .HasOne<AppUser>()
            .WithMany()
            .HasForeignKey(task => task.CreatedBy)
            .HasPrincipalKey(user => user.Id)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
            */
    }
}