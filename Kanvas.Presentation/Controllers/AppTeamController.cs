using AutoMapper;
using Presentation;
using Presentation.DTOs;
using Presentation.Mapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.DTOs.Team;
using Presentation.Entities;

namespace Presentation.Controllers;

[ApiController]
[Route("api/teams")]
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
    [Route("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        // TODO: get all from accessible teams
        var team = await _context.AppTeams.FirstOrDefaultAsync(t => t.Id == id);
        if (team == null) return NotFound();
        return Ok(_mapper.Map<AppTeamDto>(team));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // TODO: only for accessible teams
        var teams = await _context.AppTeams.ToListAsync();
        return Ok(teams.Select(team => _mapper.Map<AppTeamDto>(team)));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTeamRequestDto appTeamDto)
    {
        // TODO: use userId to ownerId
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var team = _mapper.Map<AppTeam>(appTeamDto);
        _context.AppTeams.Add(team);
        await _context.SaveChangesAsync();
        
        return Ok(_mapper.Map<AppTeamDto>(team));
    }

    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateTeamRequestDto appTeamDto)
    {
        // TODO: only for owner
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var team = _context.AppTeams.FirstOrDefault(t => t.Id == id);
        if (team == null) return NotFound();
        _mapper.Map(appTeamDto, team);
        await _context.SaveChangesAsync();
        
        return Ok(_mapper.Map<AppTeamDto>(team));
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        // TODO: only for owner
        var team = await _context.AppTeams.FirstOrDefaultAsync(t => t.Id == id);
        if (team == null) return NotFound();
        _context.AppTeams.Remove(team);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }

    [HttpPost]
    [Route("{teamId}/users/{userId}")]
    public async Task<IActionResult> AddUserToTeam([FromRoute] Guid teamId, [FromRoute] Guid userId)
    {
        // TODO: only for owner
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
        // TODO: only for owner
        var team = await _context.AppTeams.FirstOrDefaultAsync(t => t.Id == teamId);
        if (team == null) return NotFound();
        
        var membership = await _context.TeamMembers.FirstOrDefaultAsync(t => t.MemberId == userId);
        if (membership == null) return NotFound();
        
        _context.TeamMembers.Remove(membership);
        await _context.SaveChangesAsync();
        
        return Ok();
    }
    
}