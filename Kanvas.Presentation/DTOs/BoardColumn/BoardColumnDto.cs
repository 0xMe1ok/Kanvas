using Presentation.Enums;

namespace Presentation.DTOs.BoardColumn;

public class BoardColumnDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } 
    public int Order { get; set; }
    public int? TaskLimit { get; set; }
    public Status Status { get; set; }
    public Guid BoardId { get; set; }
}