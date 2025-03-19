using Microsoft.AspNetCore.Identity;
using Presentation.Entities;

namespace Presentation.Identity;

public class AppUser : IdentityUser<Guid>
{
    public List<TeamMember> Memberships { get; set; } = [];
    public List<AppTeam> Teams { get; set; } = [];
    public List<AppTask> Tasks { get; set; } = [];
}