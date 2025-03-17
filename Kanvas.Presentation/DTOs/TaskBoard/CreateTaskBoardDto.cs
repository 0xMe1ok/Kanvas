namespace Presentation.DTOs.TaskBoard;

public class CreateTaskBoardDto
{
    public string Name { get; set; }
    public Guid TeamId { get; set; }
}