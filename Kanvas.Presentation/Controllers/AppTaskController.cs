using AutoMapper;
using Presentation;
using Presentation.DTOs;
using Presentation.Mapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.Entities;
using Presentation.Mapper;

namespace Presentation.Controllers;

[ApiController]
[Route("api/task")]
public class AppTaskController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AppTaskController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    [HttpGet]
    [Route("{taskId}")]
    public async Task<IActionResult> GetById(Guid taskId)
    {
        var task = await _context.AppTasks.FirstOrDefaultAsync(t => t.Id == taskId);
        if (task == null) return NotFound();
        return Ok(_mapper.Map<AppTaskDto>(task));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tasks = await _context.AppTasks.ToListAsync();
        
        return Ok(tasks.Select(task => _mapper.Map<AppTaskDto>(task)));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateAppTaskRequestDto appTaskDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var task = _mapper.Map<AppTask>(appTaskDto);

        await _context.AppTasks.AddAsync(task);
        await _context.SaveChangesAsync();
        
        return Ok();
    }

    [HttpPut]
    [Route("{taskId}")]
    public async Task<IActionResult> Update([FromRoute] Guid taskId, [FromBody] UpdateAppTaskRequestDto appTaskDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var task = await _context.AppTasks.FirstOrDefaultAsync(t => t.Id == taskId);
        if (task == null) return NotFound();
        
        _mapper.Map(appTaskDto, task);
        
        await _context.SaveChangesAsync();
        return Ok(appTaskDto);
    }

    [HttpDelete]
    [Route("{taskId}")]
    public async Task<IActionResult> Delete([FromRoute] Guid taskId)
    {
        var task = await _context.AppTasks.FirstOrDefaultAsync(t => t.Id == taskId);
        if (task == null) return NotFound();
        _context.AppTasks.Remove(task);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}