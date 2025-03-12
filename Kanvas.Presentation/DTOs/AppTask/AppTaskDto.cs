using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;
using Presentation.Enums;

namespace Presentation.DTOs;

public class AppTaskDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? DueDate { get; set; }
    public Guid? BoardId { get; set; }
    public Guid? ColumnId { get; set; }
    
    [JsonConverter(typeof(StringEnumConverter))]
    public Status Status { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid? AssigneeId { get; set; }
}