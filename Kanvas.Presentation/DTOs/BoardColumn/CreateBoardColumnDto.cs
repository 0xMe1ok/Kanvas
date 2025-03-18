using Presentation.Enums;

namespace Presentation.DTOs.BoardColumn;

public class CreateBoardColumnDto
{
    public string Name { get; set; } 
    public int? TaskLimit { get; set; }
    public Status Status { get; set; }
}