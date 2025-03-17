using Presentation.DTOs;
using Presentation.Entities;

namespace Presentation.Interfaces;

public interface IAppTaskService
{
    Task<AppTask?> CreateNewTask(CreateAppTaskDto taskDto);
    Task<AppTask?> GetTaskAsync(Guid id);
    
    Task<IEnumerable<AppTask>> GetTasksAsync();
    
    Task UpdateTaskAsync(Guid id, UpdateAppTaskDto taskDto);
    Task DeleteTaskAsync(Guid id);
}