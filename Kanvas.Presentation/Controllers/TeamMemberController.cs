using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Presentation.Interfaces;

namespace Presentation.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/teams/{teamId}/users")]
[ApiVersion("1.0")]
public class TeamMemberController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TeamMemberController(
        IAppTeamService appTeamService,
        IUnitOfWork unitOfWork, 
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetTeamMembers(Guid teamId)
    {
        // TODO: Move to service
        var members = await _unitOfWork.TeamMembers.GetAll(teamId);
        return Ok(members);
    }

    [HttpDelete]
    [Route("{userId}")]
    public async Task<IActionResult> DeleteTeamMember(Guid teamId, Guid userId)
    {
        // TODO: Move to service
        await _unitOfWork.TeamMembers.RemoveAsync(teamId, userId);
        return NoContent();
    }
    
}