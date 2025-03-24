using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Entities;
using Presentation.Enums;
using Presentation.Exceptions;
using Presentation.Interfaces;

namespace Presentation.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/teams/{teamId}/users")]
[ApiVersion("1.0")]
[Authorize]
public class TeamMemberController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;
    private readonly ITeamRoleService _teamRoleService;


    public TeamMemberController(
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

    [HttpGet]
    public async Task<IActionResult> GetTeamMembers(Guid teamId)
    {
        // TODO : move logic to service
        var userId = _userContext.UserId;
        if (!await _unitOfWork.TeamMembers.ExistsAsync(teamId, userId))
            throw new ForbiddenException($"User {userId} does not belong to team");
        
        var members = await _unitOfWork.TeamMembers.GetAll(teamId);
        return Ok(members);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateTeamMember(TeamMember teamMember)
    {
        var userId = _userContext.UserId;
        
        if (!await _unitOfWork.TeamMembers.ExistsAsync(teamMember.TeamId, userId))
            throw new ForbiddenException($"User {userId} does not belong to team");
        
        if (!await _teamRoleService.IsInTeamRoleOrHigherAsync(userId, teamMember.TeamId, TeamRole.Admin))
            throw new ForbiddenException($"User does not have permission to update user in team {teamMember.TeamId}");
        
        var member = await _unitOfWork.TeamMembers.GetByIdAsync(teamMember.TeamId, teamMember.MemberId);
        if (member == null)
            throw new ForbiddenException($"User {userId} does not belong to team");
        
        member.Role = teamMember.Role;
        _unitOfWork.TeamMembers.Update(member);
        await _unitOfWork.CommitAsync();
        return Ok(teamMember);
    }

    [HttpDelete]
    [Route("{userId}")]
    public async Task<IActionResult> DeleteTeamMember(Guid teamId, Guid userIdToDelete)
    {
        // TODO : move logic to service
        var userId = _userContext.UserId;
        if (!await _unitOfWork.TeamMembers.ExistsAsync(teamId, userId))
            throw new ForbiddenException($"User {userId} does not belong to team");
        
        if (!await _teamRoleService.IsInTeamRoleOrHigherAsync(userId, teamId, TeamRole.Admin))
            throw new ForbiddenException($"User does not have permission to delete user in team {teamId}");
        
        await _unitOfWork.TeamMembers.RemoveAsync(teamId, userIdToDelete);
        return NoContent();
    }
    
}