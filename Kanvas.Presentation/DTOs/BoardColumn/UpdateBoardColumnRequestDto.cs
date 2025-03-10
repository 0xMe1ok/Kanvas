using Presentation.Enums;

namespace Presentation.DTOs.BoardColumn;

public class UpdateBoardColumnRequestDto
{
    public string Name { get; set; } 
    public int? TaskLimit { get; set; }
    public Status Status { get; set; }
    public Guid BoardId { get; set; }
}