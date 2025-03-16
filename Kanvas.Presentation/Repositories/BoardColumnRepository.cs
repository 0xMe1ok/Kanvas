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
}