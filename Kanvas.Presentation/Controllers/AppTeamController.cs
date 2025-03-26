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
        var team = await _appTeamService.GetTeamAsync(id);
        return Ok(_mapper.Map<AppTeamDto>(team));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var teams = await _appTeamService.GetTeamsAsync();
        return Ok(teams.Select(team => _mapper.Map<AppTeamDto>(team)));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAppTeamDto teamDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var team = await _appTeamService.CreateNewTeam(teamDto);
        return CreatedAtAction(nameof(GetById), new { id = team.Id }, _mapper.Map<AppTeamDto>(team));
    }

    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateAppTeamDto teamDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _appTeamService.UpdateTeamAsync(id, teamDto);
        return Ok(_mapper.Map<AppTeamDto>(teamDto));
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _appTeamService.DeleteTeamAsync(id);
        return NoContent();
    }
}