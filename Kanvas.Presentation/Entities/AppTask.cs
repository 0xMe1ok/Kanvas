using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Presentation.Enums;

namespace Presentation.Entities;

public class AppTask : EntityBase<Guid>
{
    public string Name { get; set; }
    
    public string Description { get; set; }

    public int Order { get; set; } = 0;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DueDate { get; set; }
    
    public Guid? BoardId { get; set; }
    public Guid? ColumnId { get; set; }
    public Status Status { get; set; } = Status.ToDo;
    public Guid CreatedBy { get; set; }
    public Guid? AssigneeId { get; set; }
    
    public TaskBoard? Board { get; set; }
    public BoardColumn? Column { get; set; }

    public void SetAssignedTo(Guid? assignedTo)
    {
        AssigneeId = assignedTo;
    }

    public void SetDueDate(DateTime? dueDate)
    {
        DueDate = dueDate;
    }

    public void SetName(string name)
    {
        Name = name;
    }

    public void SetDescription(string description)
    {
        Description = description;
    }

    public void SetOrder(int order)
    {
        Order = order;
    }
}