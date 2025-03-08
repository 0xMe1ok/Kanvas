using AutoMapper;
using Infrastructure;
using Presentation.DTOs;
using Infrastructure.Mapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.DTOs.Team;
using Presentation.Entities;

namespace Presentation.Controllers;

[ApiController]
[Route("api/team")]
public class AppTeamController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AppTeamController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("{teamId}")]
    public async Task<IActionResult> GetById([FromRoute] Guid teamId)
    {
        var team = await _context.AppTeams.FirstOrDefaultAsync(t => t.Id == teamId);
        if (team == null) return NotFound();
        return Ok(_mapper.Map<AppTeamDto>(team));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var teams = await _context.AppTeams.ToListAsync();
        return Ok(teams.Select(team => _mapper.Map<AppTeamDto>(team)));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateAppTaskRequestDto appTeamDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var team = _mapper.Map<AppTeam>(appTeamDto);
        _context.AppTeams.Add(team);
        await _context.SaveChangesAsync();
        return Ok(_mapper.Map<AppTeamDto>(team));
    }

    [HttpPut]
    [Route("{teamId}")]
    public async Task<IActionResult> Put([FromRoute] Guid teamId, [FromBody] UpdateAppTaskRequestDto appTeamDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var team = _context.AppTeams.FirstOrDefault(t => t.Id == teamId);
        if (team == null) return NotFound();
        
        _mapper.Map(appTeamDto, team);
        
        await _context.SaveChangesAsync();
        return Ok(_mapper.Map<AppTeamDto>(team));
    }

    [HttpDelete]
    [Route("{teamId}")]
    public async Task<IActionResult> Delete([FromRoute] Guid teamId)
    {
        var team = await _context.AppTeams.FirstOrDefaultAsync(t => t.Id == teamId);
        if (team == null) return NotFound();
        _context.AppTeams.Remove(team);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost]
    [Route("{teamId}/users/{userId}")]
    public async Task<IActionResult> AddUserToTeam([FromRoute] Guid teamId, [FromRoute] Guid userId)
    {
        var team = await _context.AppTeams.FirstOrDefaultAsync(t => t.Id == teamId);
        if (team == null) return NotFound();

        var membership = new TeamMember
        {
            MemberId = userId,
            TeamId = teamId
        };
        
        _context.TeamMembers.Add(membership);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete]
    [Route("{teamId}/users/{userId}")]
    public async Task<IActionResult> RemoveUserFromTeam([FromRoute] Guid teamId, [FromRoute] Guid userId)
    {
        var team = await _context.AppTeams.FirstOrDefaultAsync(t => t.Id == teamId);
        if (team == null) return NotFound();
        var membership = await _context.TeamMembers.FirstOrDefaultAsync(t => t.MemberId == userId);
        if (membership == null) return NotFound();
        _context.TeamMembers.Remove(membership);
        await _context.SaveChangesAsync();
        return Ok();
    }
    
}