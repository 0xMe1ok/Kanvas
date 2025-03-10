namespace Presentation.Entities;

public class TaskBoard : EntityBase<Guid>
{
    public string Name { get; set; }
    public Guid TeamId { get; set; }
    public AppTeam Team { get; set; }
    public List<BoardColumn> Columns { get; set; }
    
    public List<AppTask> Tasks { get; set; }
}