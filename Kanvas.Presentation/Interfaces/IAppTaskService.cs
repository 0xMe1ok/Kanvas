using Presentation.DTOs;
using Presentation.Entities;
using Presentation.Enums;

namespace Presentation.Interfaces;

public interface IAppTaskService
{
    Task<AppTask?> CreateNewTask(Guid teamId, CreateAppTaskDto taskDto);
    Task<AppTask?> GetTaskAsync(Guid teamId, Guid id);
    
    Task<IEnumerable<AppTask>> GetTasksAsync(Guid teamId);
    
    Task UpdateTaskAsync(Guid teamId, Guid id, UpdateAppTaskDto taskDto);
    Task DeleteTaskAsync(Guid teamId, Guid id);
    
    Task ChangeTaskStatusAsync(Guid teamId, Guid id, Status newStatus);
    
    Task MoveTaskAsync(Guid teamId, Guid id, int newOrder);
}