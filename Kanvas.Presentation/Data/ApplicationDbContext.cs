using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Presentation.Entities;
using Presentation.Identity;
using Presentation.Identity.Tokens;

namespace Presentation.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, Guid, AppUserClaim, 
    AppUserRole, AppUserLogin, AppRoleClaim, AppUserToken>
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
        
    }
    
    public DbSet<TeamMember> TeamMembers { get; set; }
    public DbSet<AppTeam> AppTeams { get; set; }
    public DbSet<TaskBoard> TaskBoards { get; set; }
    public DbSet<BoardColumn> BoardColumns { get; set; }
    public DbSet<AppTask> AppTasks { get; set; }
    
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<InviteToken> InviteTokens { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}