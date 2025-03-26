using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Presentation.DTOs.Team;
using Presentation.Entities;
using Presentation.Enums;
using Presentation.Exceptions;
using Presentation.Identity;
using Presentation.Interfaces;
using Presentation.Interfaces.Service;

namespace Presentation.Services;

public class AppTeamService : IAppTeamService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;
    private readonly ITeamRoleService _teamRoleService;

    public AppTeamService(
        IUnitOfWork unitOfWork, 
        IMapper mapper, 
        IUserContext userContext,
        ITeamRoleService teamRoleService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userContext = userContext;
        _teamRoleService = teamRoleService;
    }
    public async Task<AppTeam?> CreateNewTeam(CreateAppTeamDto teamDto)
    {
        var userId = _userContext.UserId;
        
        var team = _mapper.Map<AppTeam>(teamDto);
        team.OwnerId = userId;
        
        await _unitOfWork.Teams.AddAsync(team);
        
        var membership = new TeamMember()
        {
            TeamId = team.Id,
            MemberId = userId,
            Role = TeamRole.Owner
        };
        
        await _unitOfWork.TeamMembers.AddAsync(membership);
        await _unitOfWork.CommitAsync();
        
        return _mapper.Map<AppTeam>(team);
    }

    public async Task<AppTeam?> GetTeamAsync(Guid id)
    {
        var userId = _userContext.UserId;

        if (!await _unitOfWork.TeamMembers.ExistsAsync(id, userId))
            throw new ForbiddenException($"User {userId} does not belong to team");
        
        var team = await _unitOfWork.Teams.GetByIdAsync(id);
        if (team == null) throw new NotFoundException("Team not found");
        return team;
    }

    public async Task<IEnumerable<AppTeam>> GetTeamsAsync()
    {
        var userId = _userContext.UserId;
        var teamIds = await _unitOfWork.TeamMembers.GetTeamsIdAsync(userId);
        
        return await _unitOfWork.Teams.FindAllAsync(t => teamIds.Contains(t.Id));
    }

    public async Task UpdateTeamAsync(Guid id, UpdateAppTeamDto teamDto)
    {
        var userId = _userContext.UserId;
        if (!await _unitOfWork.TeamMembers.ExistsAsync(id, userId))
            throw new ForbiddenException($"User {userId} does not belong to team");
        
        if (!await _teamRoleService.IsInTeamRoleOrHigherAsync(userId, id, TeamRole.Admin))
            throw new ForbiddenException($"User does not have permission to update team {id}");
        
        var team = await _unitOfWork.Teams.GetByIdAsync(id);
        if (team == null) throw new NotFoundException("Team not found");
        _mapper.Map(teamDto, team);
        await _unitOfWork.CommitAsync();
    }

    public async Task DeleteTeamAsync(Guid id)
    {
        var userId = _userContext.UserId;
        if (!await _unitOfWork.TeamMembers.ExistsAsync(id, userId))
            throw new ForbiddenException($"User {userId} does not belong to team");

        if (!await _teamRoleService.IsInTeamRoleOrHigherAsync(userId, id, TeamRole.Owner))
            throw new ForbiddenException($"User does not have permission to delete team {id}");
        
        var team = await _unitOfWork.Teams.GetByIdAsync(id);
        if (team == null) throw new NotFoundException("Team not found");
        _unitOfWork.Teams.Remove(team);
        await _unitOfWork.CommitAsync();
    }
}