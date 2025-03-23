namespace Presentation.Entities;

public class InviteToken
{
    public Guid Id { get; set; }
    
    public required string Token { get; set; }
    
    public Guid TeamId { get; set; }
    
    public DateTime Expires { get; set; }
    
    public AppTeam Team { get; set; }
}