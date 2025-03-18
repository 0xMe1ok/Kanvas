using Presentation.Entities;

namespace Presentation.Interfaces;

public interface IBoardColumnRepository : IRepository<BoardColumn>
{
    Task AddRangeAsync(IEnumerable<BoardColumn> boardColumns);
    Task<int> GetMaxOrderInBoardAsync(Guid boardId);

    Task ShiftColumnsOrderAsync(
        Guid boardId,
        int startOrder,
        int endOrder,
        int shiftBy);
}