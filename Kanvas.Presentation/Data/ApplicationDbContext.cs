using Presentation.Entities;
using Microsoft.EntityFrameworkCore;
using Presentation.Configuration;

namespace Presentation;

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
        // TODO: change OwnerId, MemberId and other userId's to user FK
    }
}