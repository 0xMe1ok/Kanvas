using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Presentation.Entities;

namespace Presentation.Configuration;

public class BoardColumnConfiguration : IEntityTypeConfiguration<BoardColumn>
{
    public void Configure(EntityTypeBuilder<BoardColumn> builder)
    {
        builder.ToTable("Columns");
        builder.HasKey(column => column.Id);
        builder.Property(column => column.Id).ValueGeneratedOnAdd();
        builder
            .Property(c => c.Status)
            .HasConversion<string>();
    }
}