using Presentation.Entities;
using Presentation.Interfaces;

namespace Presentation.Repositories;

public class BoardColumnRepository : Repository<BoardColumn>, IBoardColumnRepository
{
    public BoardColumnRepository(ApplicationDbContext context) : base(context)
    {
    }
}