using Domain.Entities;
using Infrastructure.DTOs;

namespace Infrastructure.Mapper;

public static class AppTaskMapper
{
    public static AppTaskDto ToAppTaskDto(this AppTask appTask)
    {
        return new AppTaskDto
        {
            Id = appTask.Id,
            Name = appTask.Name,
            Description = appTask.Description,
            DueDate = appTask.DueDate,
            CreatedAt = appTask.CreatedAt,
            ColumnId = appTask.ColumnId,
            CreatedBy = appTask.CreatedBy,
            AssigneeId = appTask.AssigneeId,
        };
    }

    public static AppTask ToAppTask(this CreateAppTaskRequestDto appTaskDto)
    {
        return new AppTask
        {
            Id = Guid.NewGuid(),
            Name = appTaskDto.Name,
            Description = appTaskDto.Description,
            DueDate = appTaskDto.DueDate,
            AssigneeId = appTaskDto.AssigneeId,
            ColumnId = appTaskDto.ColumnId,
        };
    }
}