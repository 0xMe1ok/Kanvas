using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Presentation.Entities;
using Presentation.Identity;

namespace Presentation.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, Guid>
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