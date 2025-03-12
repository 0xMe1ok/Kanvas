using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Presentation.Entities;
using Presentation.Enums;

namespace Presentation.DTOs.BoardColumn;

public class BoardColumnDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } 
    public int Order { get; set; }
    public int? TaskLimit { get; set; }
    
    [JsonConverter(typeof(StringEnumConverter))]
    public Status Status { get; set; }
    public Guid BoardId { get; set; }
    
    public List<AppTaskDto> Tasks { get; set; }
}