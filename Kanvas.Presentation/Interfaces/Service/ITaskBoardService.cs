using Presentation.DTOs.TaskBoard;
using Presentation.Entities;

namespace Presentation.Interfaces.Service;

public interface ITaskBoardService
{
    Task<TaskBoard> CreateNewBoard(Guid teamId, CreateTaskBoardDto boardDto);
    Task<TaskBoard?> GetBoardAsync(Guid teamId, Guid boardId);
    
    Task<IEnumerable<TaskBoard>> GetBoardsAsync();
    Task<IEnumerable<TaskBoard>> GetBoardsAsync(Guid teamId);
    Task UpdateBoardAsync(Guid teamId, Guid id, UpdateTaskBoardDto boardDto);
    Task DeleteBoardAsync(Guid teamId, Guid id);
}