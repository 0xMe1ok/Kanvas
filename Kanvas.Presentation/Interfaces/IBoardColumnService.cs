using Presentation.DTOs.BoardColumn;
using Presentation.Entities;

namespace Presentation.Interfaces;

public interface IBoardColumnService
{
    Task<BoardColumn?> CreateNewColumn(Guid boardId, CreateBoardColumnDto columnDto);
    Task<IEnumerable<BoardColumn>> GetColumnsAsync(Guid boardId);
    
    Task UpdateColumnAsync(Guid boardId, Guid columnId, UpdateBoardColumnDto columnDto);
    Task DeleteColumnAsync(Guid boardId, Guid columnId);
    
    Task MoveColumnAsync(Guid boardId, Guid columnId, int newOrder);
}