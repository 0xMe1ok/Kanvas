namespace Presentation.DTOs.TaskBoard;

public class CreateTaskBoardRequestDto
{
    public string Name { get; private set; }
    public Guid TeamId { get; private set; }
}