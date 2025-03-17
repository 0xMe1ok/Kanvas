using Presentation.DTOs.TaskBoard;
using Presentation.Entities;

namespace Presentation.DTOs.Team;

public class AppTeamDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string OwnerId { get; set; }
    public List<TaskBoardDto>? Boards { get; set; }
}