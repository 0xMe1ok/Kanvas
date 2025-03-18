using Presentation.DTOs.BoardColumn;
using Presentation.Entities;
using Presentation.Interfaces;

namespace Presentation.Services;

public class BoardColumnService : IBoardColumnService
{
    // TODO: create logic here
    public Task<BoardColumn?> CreateNewColumn(CreateBoardColumnDto boardDto)
    {
        throw new NotImplementedException();
    }

    public Task<BoardColumn?> GetColumnAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<BoardColumn>> GetColumnsAsync()
    {
        throw new NotImplementedException();
    }

    public Task UpdateColumnAsync(Guid id, UpdateBoardColumnDto boardDto)
    {
        throw new NotImplementedException();
    }

    public Task DeleteColumnAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}