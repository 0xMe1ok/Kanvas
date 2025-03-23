using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Presentation;
using Presentation.DTOs;
using Presentation.Mapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.DTOs.TaskBoard;
using Presentation.DTOs.Team;
using Presentation.Entities;
using Presentation.Enums;
using Presentation.Interfaces;

namespace Presentation.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/teams")]
[ApiVersion("1.0")]
[Authorize]
public class AppTeamController : ControllerBase
{
    private readonly IAppTeamService _appTeamService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AppTeamController(
        IAppTeamService appTeamService,
        IUnitOfWork unitOfWork, 
        IMapper mapper)
    {
        _appTeamService = appTeamService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        // TODO: get all from accessible teams
        var team = await _appTeamService.GetTeamAsync(id);
        return Ok(_mapper.Map<AppTeamDto>(team));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // TODO: only for accessible teams
        var teams = await _appTeamService.GetTeamsAsync();
        return Ok(teams.Select(team => _mapper.Map<AppTeamDto>(team)));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAppTeamDto teamDto)
    {
        // TODO: use userId to ownerId
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var team = await _appTeamService.CreateNewTeam(teamDto);
        return Ok(_mapper.Map<AppTeamDto>(team));
    }

    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateAppTeamDto teamDto)
    {
        // TODO: only for owner
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _appTeamService.UpdateTeamAsync(id, teamDto);
        return Ok(_mapper.Map<AppTeamDto>(teamDto));
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        // TODO: only for owner
        await _appTeamService.DeleteTeamAsync(id);
        return NoContent();
    }
    
    // TODO: do when user was added
    [HttpPost]
    [Route("{teamId}/users")] // [Route("{teamId}/users")]
    public async Task<IActionResult> AddUserToTeam([FromRoute] Guid teamId, [FromRoute] Guid userId) // FromBody] user UsedId
    {
        // TODO: only for owner
        return Ok();
    }
    
    // TODO: do when user was added
    [HttpDelete]
    [Route("{teamId}/users/{userId}")]
    public async Task<IActionResult> RemoveUserFromTeam([FromRoute] Guid teamId, [FromRoute] Guid userId)
    {
        // TODO: only for owner
        return Ok();
    }
    
}