using Presentation.DTOs.BoardColumn;
using Presentation.Entities;

namespace Presentation.Interfaces;

public interface IBoardColumnService
{
    Task<BoardColumn?> CreateNewColumn(Guid teamId, Guid boardId, CreateBoardColumnDto columnDto);
    Task<IEnumerable<BoardColumn>> GetColumnsAsync(Guid teamId, Guid boardId);
    
    Task UpdateColumnAsync(Guid teamId, Guid boardId, Guid columnId, UpdateBoardColumnDto columnDto);
    Task DeleteColumnAsync(Guid teamId, Guid boardId, Guid columnId);
    
    Task MoveColumnAsync(Guid teamId, Guid boardId, Guid columnId, int newOrder);
}