using Presentation.Entities;
using Presentation.Interfaces;

namespace Presentation.Repositories;

public class AppTaskRepository : Repository<AppTask>, IAppTaskRepository
{
    public AppTaskRepository(ApplicationDbContext context) : base(context)
    {
    }
}