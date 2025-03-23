using Microsoft.EntityFrameworkCore;
using Presentation.Data;
using Presentation.Entities;
using Presentation.Interfaces;
using Presentation.Interfaces.Repository;

namespace Presentation.Repositories;

public class AppTeamRepository : Repository<AppTeam>, IAppTeamRepository
{
    public AppTeamRepository(ApplicationDbContext context) 
        : base(context)
    {
    }
    
    
}