using Presentation.Entities;

namespace Presentation.DTOs.TaskBoard;

public class TaskBoardDto
{
    public string Name { get; private set; }
    public Guid TeamId { get; private set; }
    public List<Entities.BoardColumn> Columns { get; private set; }
}