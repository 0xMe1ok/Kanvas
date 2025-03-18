using Presentation.Enums;

namespace Presentation.DTOs.BoardColumn;

public class UpdateBoardColumnDto
{
    public string Name { get; set; } 
    public int? TaskLimit { get; set; }
    public Status Status { get; set; }
    public int Order { get; set; }
}