namespace Domain.Entities;

public class TaskBoard : EntityBase<Guid>
{
    public string Name { get; private set; }
    public Guid TeamId { get; private set; }
    public AppTeam Team { get; private set; }
    public List<BoardColumn> Columns { get; private set; }
}