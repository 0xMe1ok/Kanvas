using Presentation.Entities;

namespace Presentation.DTOs.Team;

public class AppTeamDto
{
    public string Name { get; private set; }
    
    public string OwnerId { get; private set; }
    
    public List<Entities.TaskBoard>? Boards { get; private set; }
}