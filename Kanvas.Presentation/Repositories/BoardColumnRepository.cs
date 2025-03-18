using Microsoft.EntityFrameworkCore;
using Presentation.Entities;
using Presentation.Interfaces;

namespace Presentation.Repositories;

public class BoardColumnRepository : Repository<BoardColumn>, IBoardColumnRepository
{
    public BoardColumnRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override Task<BoardColumn?> GetByIdAsync(Guid id)
    {
        return _context.Set<BoardColumn>()
            .Include(column => column.Tasks)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddRangeAsync(IEnumerable<BoardColumn> boardColumns)
    {
        await _context.Set<BoardColumn>().AddRangeAsync(boardColumns);
    }
    
    public async Task<int> GetMaxOrderInBoardAsync(Guid boardId)
    {
        return await _context.BoardColumns
            .Where(c => c.BoardId == boardId)
            .MaxAsync(c => (int?)c.Order) ?? 0;
    }
    
    public async Task ShiftColumnsOrderAsync(
        Guid boardId,
        int startOrder,
        int endOrder,
        int shiftBy)
    {
        await _context.BoardColumns
            .Where(c => c.BoardId == boardId)
            .Where(c => c.Order >= startOrder && c.Order <= endOrder)
            .ExecuteUpdateAsync(setters => 
                setters.SetProperty(
                    c => c.Order,
                    c => c.Order + shiftBy));
    }
}