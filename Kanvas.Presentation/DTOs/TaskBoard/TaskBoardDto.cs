using Presentation.DTOs.BoardColumn;
using Presentation.Entities;

namespace Presentation.DTOs.TaskBoard;

public class TaskBoardDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid TeamId { get; set; }
    public List<BoardColumnDto> Columns { get; set; }
}