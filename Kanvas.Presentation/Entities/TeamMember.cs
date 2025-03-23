using Presentation.Enums;

namespace Presentation.Entities;

public class TeamMember
{
    public Guid TeamId { get; set; }
    public Guid MemberId { get; set; }
    
    public TeamRole Role { get; set; }
}