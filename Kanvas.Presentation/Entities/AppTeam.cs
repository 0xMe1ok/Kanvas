namespace Presentation.Entities;

public class AppTeam : EntityBase<Guid>
{
    public string Name { get; private set; }
    
    public Guid OwnerId { get; private set; }
    public List<TaskBoard> Boards { get; private set; }
}