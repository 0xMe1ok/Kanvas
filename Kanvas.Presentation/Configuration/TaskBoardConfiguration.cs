using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Presentation.Entities;

namespace Presentation.Configuration;

public class TaskBoardConfiguration : IEntityTypeConfiguration<TaskBoard>
{
    public void Configure(EntityTypeBuilder<TaskBoard> builder)
    {
        builder.ToTable("Boards");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).ValueGeneratedOnAdd();
        builder
            .HasMany(board => board.Columns)
            .WithOne(column => column.Board)
            .HasForeignKey(board => board.BoardId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}