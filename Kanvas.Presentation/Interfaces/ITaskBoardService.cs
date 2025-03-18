using Presentation.DTOs.BoardColumn;
using Presentation.DTOs.TaskBoard;
using Presentation.Entities;

namespace Presentation.Interfaces;

public interface ITaskBoardService
{
    Task<TaskBoard?> CreateNewBoard(CreateTaskBoardDto boardDto);
    Task<TaskBoard?> GetBoardAsync(Guid id);
    
    Task<IEnumerable<TaskBoard>> GetBoardAsync();
    
    Task UpdateBoardAsync(Guid id, UpdateTaskBoardDto boardDto);
    Task DeleteBoardAsync(Guid id);
    
    Task<bool> IsBoardExistsAsync(Guid id);
    Task<bool> IsBoardAccessibleAsync(Guid id);
}