using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Presentation.Enums;
using Presentation.Exceptions;
using Presentation.Interfaces;

namespace Presentation.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/teams/{teamId}/users")]
[ApiVersion("1.0")]
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