using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
        
    }
    
    public DbSet<TeamMember> TeamMembers { get; set; }
    public DbSet<AppTeam> AppTeams { get; set; }
    
    public DbSet<TaskBoard> TaskBoards { get; set; }
    public DbSet<BoardColumn> BoardColumns { get; set; }
    public DbSet<AppTask> AppTasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        // TODO: change OwnerId, MemberId and other to user FK
        
        modelBuilder.Entity<TeamMember>().ToTable("TeamMembers");
        modelBuilder.Entity<TeamMember>().HasKey(p => new {p.MemberId, p.TeamId}); // user here
        modelBuilder.Entity<TeamMember>().Property(p => p.MemberId).ValueGeneratedOnAdd();

        modelBuilder.Entity<AppTeam>().ToTable("Teams");
        modelBuilder.Entity<AppTeam>().HasKey(team => team.Id);
        modelBuilder.Entity<AppTeam>().Property(team => team.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<AppTeam>().Property(team => team.OwnerId).HasDefaultValue(Guid.NewGuid()); // user here
        modelBuilder.Entity<AppTeam>()
            .HasMany(team => team.Boards)
            .WithOne(board => board.Team)
            .HasForeignKey(t => t.TeamId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<TaskBoard>().ToTable("Boards");
        modelBuilder.Entity<TaskBoard>().HasKey(t => t.Id);
        modelBuilder.Entity<TaskBoard>().Property(t => t.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<TaskBoard>()
            .HasMany(board => board.Columns)
            .WithOne(column => column.Board)
            .HasForeignKey(board => board.BoardId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<BoardColumn>().ToTable("Columns");
        modelBuilder.Entity<BoardColumn>().HasKey(column => column.Id);
        modelBuilder.Entity<BoardColumn>().Property(column => column.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<BoardColumn>()
            .HasMany(column => column.Tasks)
            .WithOne(task => task.Column)
            .HasForeignKey(task => task.ColumnId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<AppTask>().ToTable("Tasks");
        modelBuilder.Entity<AppTask>().HasKey(t => t.Id);
        modelBuilder.Entity<AppTask>().Property(t => t.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<AppTask>().Property(t => t.CreatedBy).HasDefaultValue(Guid.NewGuid()); // user here
    }
}