using Presentation.Enums;

namespace Presentation.Entities;

public class BoardColumn : EntityBase<Guid>
{
    public string Name { get; private set; } 
    public int Order { get; private set; }
    public int? TaskLimit { get; private set; }
    public Status Status { get; set; }
    
    public Guid BoardId { get; private set; }
    public TaskBoard Board { get; private set; }
}