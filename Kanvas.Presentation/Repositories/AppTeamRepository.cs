using Microsoft.EntityFrameworkCore;
using Presentation.Entities;
using Presentation.Interfaces;

namespace Presentation.Repositories;

public class AppTeamRepository : Repository<AppTeam>, IAppTeamRepository
{
    public AppTeamRepository(ApplicationDbContext context) 
        : base(context)
    {
    }
}