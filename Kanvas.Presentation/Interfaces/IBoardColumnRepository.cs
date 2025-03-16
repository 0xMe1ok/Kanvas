using Presentation.Entities;

namespace Presentation.Interfaces;

public interface IBoardColumnRepository : IRepository<BoardColumn>
{
    Task AddRangeAsync(IEnumerable<BoardColumn> boardColumns);
}