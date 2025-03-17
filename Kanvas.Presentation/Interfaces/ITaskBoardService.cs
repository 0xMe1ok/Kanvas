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
    
    // Columns
    
    Task<BoardColumn?> CreateNewColumn(CreateBoardColumnDto boardDto);
    Task<BoardColumn?> GetColumnAsync(Guid id);
    
    Task<IEnumerable<BoardColumn>> GetColumnsAsync();
    
    Task UpdateColumnAsync(Guid id, UpdateBoardColumnDto boardDto);
    Task DeleteColumnAsync(Guid id);
}