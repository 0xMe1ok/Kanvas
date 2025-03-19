using AutoMapper;
using Presentation.DTOs.Team;
using Presentation.Entities;
using Presentation.Exceptions;
using Presentation.Interfaces;

namespace Presentation.Services;

public class AppTeamService : IAppTeamService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AppTeamService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<AppTeam?> CreateNewTeam(CreateAppTeamDto teamDto)
    {
        var team = _mapper.Map<AppTeam>(teamDto);
        await _unitOfWork.Teams.AddAsync(team);
        await _unitOfWork.CommitAsync();
        
        return _mapper.Map<AppTeam>(team);
    }

    public async Task<AppTeam?> GetTeamAsync(Guid id)
    {
        var team = await _unitOfWork.Teams.GetByIdAsync(id);
        if (team == null) throw new NotFoundException("Team not found");
        return team;
    }

    public async Task<IEnumerable<AppTeam>> GetTeamsAsync()
    {
        return await _unitOfWork.Teams.GetAllAsync();
    }

    public async Task UpdateTeamAsync(Guid id, UpdateAppTeamDto teamDto)
    {
        var team = await _unitOfWork.Teams.GetByIdAsync(id);
        if (team == null) throw new NotFoundException("Team not found");
        _mapper.Map(teamDto, team);
        await _unitOfWork.CommitAsync();
    }

    public async Task DeleteTeamAsync(Guid id)
    {
        var team = await _unitOfWork.Teams.GetByIdAsync(id);
        if (team == null) throw new NotFoundException("Team not found");
        _unitOfWork.Teams.Remove(team);
        await _unitOfWork.CommitAsync();
    }

    public Task<bool> IsTeamExistsAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}