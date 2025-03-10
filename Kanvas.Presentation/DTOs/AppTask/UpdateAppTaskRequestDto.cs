using Presentation.Enums;

namespace Presentation.DTOs;

public class UpdateAppTaskRequestDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Order { get; set; }
    
    public DateTime? DueDate { get; set; }
    public Guid? BoardId { get; set; }
    public Status Status { get; set; }
    public Guid? AssigneeId { get; set; }
}