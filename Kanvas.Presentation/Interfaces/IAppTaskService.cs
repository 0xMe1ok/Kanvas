using Presentation.DTOs;
using Presentation.Entities;
using Presentation.Enums;

namespace Presentation.Interfaces;

public interface IAppTaskService
{
    Task<AppTask?> CreateNewTask(CreateAppTaskDto taskDto);
    Task<AppTask?> GetTaskAsync(Guid id);
    
    Task<IEnumerable<AppTask>> GetTasksAsync();
    
    Task UpdateTaskAsync(Guid id, UpdateAppTaskDto taskDto);
    Task DeleteTaskAsync(Guid id);
    
    Task ChangeTaskStatusAsync(Guid id, Status newStatus);
    
    Task MoveTaskAsync(Guid id, int newOrder);
    
    //Task UpdateTaskStatusAsync(Guid id, UpdateAppTaskStatusDto taskDto);
}