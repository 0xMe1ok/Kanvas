using Infrastructure;
using Presentation.DTOs;
using Infrastructure.Mapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Controllers;

[ApiController]
[Route("api/team")]
public class AppTeamController : ControllerBase
{
    public ApplicationDbContext _context;

    public AppTeamController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Route("{teamId}")]
    public async Task<IActionResult> GetById(Guid teamId)
    {
        var team = await _context.AppTeams.FirstOrDefaultAsync(t => t.Id == teamId);
        if (team == null) return NotFound();
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var teams = await _context.AppTeams.ToListAsync();
        return Ok(teams);
    }
    
}