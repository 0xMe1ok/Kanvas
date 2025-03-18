using Presentation.DTOs.BoardColumn;
using Presentation.Entities;

namespace Presentation.Interfaces;

public interface IBoardColumnService
{
    Task<BoardColumn?> CreateNewColumn(CreateBoardColumnDto boardDto);
    Task<BoardColumn?> GetColumnAsync(Guid id);
    
    Task<IEnumerable<BoardColumn>> GetColumnsAsync();
    
    Task UpdateColumnAsync(Guid id, UpdateBoardColumnDto boardDto);
    Task DeleteColumnAsync(Guid id);
}