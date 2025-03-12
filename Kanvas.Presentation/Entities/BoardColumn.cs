using Presentation.Enums;

namespace Presentation.Entities;

public class BoardColumn : EntityBase<Guid>
{
    public string Name { get; set; } 
    public int Order { get; set; }
    public int? TaskLimit { get; set; }
    public Status Status { get; set; }
    
    public Guid BoardId { get; set; }
    public TaskBoard Board { get; set; }
    
    public List<AppTask> Tasks { get; set; }
}