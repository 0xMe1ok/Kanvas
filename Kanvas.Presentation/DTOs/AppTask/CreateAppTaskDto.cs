using Presentation.Enums;

namespace Presentation.DTOs;

public class CreateAppTaskDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime? DueDate { get; set; }
    
    public Guid? BoardId { get; set; }
    public Guid? AssigneeId { get; set; }
}