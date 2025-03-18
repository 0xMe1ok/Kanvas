using Presentation.DTOs.BoardColumn;
using Presentation.DTOs.TaskBoard;
using Presentation.Entities;

namespace Presentation.Interfaces;

public interface ITaskBoardService
{
    Task<TaskBoard?> CreateNewBoard(CreateTaskBoardDto boardDto);
    Task<TaskBoard?> GetBoardAsync(Guid boardId);
    
    Task<IEnumerable<TaskBoard>> GetBoardsAsync();
    
    Task UpdateBoardAsync(Guid id, UpdateTaskBoardDto boardDto);
    Task DeleteBoardAsync(Guid id);
    
    Task<bool> IsBoardExistsAsync(Guid id);
    Task<bool> IsBoardAccessibleAsync(Guid id);
}