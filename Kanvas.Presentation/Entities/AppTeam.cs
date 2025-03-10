namespace Presentation.Entities;

public class AppTeam : EntityBase<Guid>
{
    public string Name { get; set; }
    
    public Guid OwnerId { get; set; }
    public List<TaskBoard>? Boards { get; set; }
}