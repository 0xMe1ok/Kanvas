using System.Text.Json.Serialization;

namespace Presentation.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Status
{
    ToDo = 0,
    InProgress = 1,
    Done = 2,
    OnHold = 3,
    Cancelled = 4
}