namespace Presentation.DTOs.TaskBoard;

public class CreateTaskBoardRequestDto
{
    public string Name { get; set; }
    public Guid TeamId { get; set; }
}