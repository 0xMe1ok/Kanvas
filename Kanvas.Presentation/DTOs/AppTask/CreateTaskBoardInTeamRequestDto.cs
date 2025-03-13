namespace Presentation.DTOs;

public class CreateTaskBoardInTeamRequestDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime? DueDate { get; set; }
    public Guid? AssigneeId { get; set; }
}