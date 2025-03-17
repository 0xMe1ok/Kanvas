using Presentation.Enums;

namespace Presentation.DTOs.BoardColumn;

public class UpdateBoardColumnDto
{
    public string Name { get; set; } 
    public int? TaskLimit { get; set; }
    public Status Status { get; set; }
    public Guid BoardId { get; set; }
}