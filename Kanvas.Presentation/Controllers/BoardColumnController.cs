using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.DTOs.BoardColumn;
using Presentation.Entities;

namespace Presentation.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/columns")]
[ApiVersion("1.0")]
public class BoardColumnController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public BoardColumnController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    [HttpGet]
    [Route("{id:guid}")]
    public async Task<ActionResult> GetById([FromRoute] Guid id)
    {
        // TODO: only for boards in accessible teams
        var column = await _context.BoardColumns.FirstOrDefaultAsync(c => c.Id == id);
        if (column == null) return NotFound();
        return Ok(_mapper.Map<BoardColumnDto>(column));
    }

    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        // TODO: only for boards in accessible teams
        var columns = _context.BoardColumns.Include(c => c.Tasks).ToList();
        return Ok(_mapper.Map<List<BoardColumnDto>>(columns));
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateBoardColumnRequestDto boardColumnDto)
    {
        // TODO: only for boards in accessible teams, for admins/redactors
        // TODO: check for valid status, there can't be two columns with the same status
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var column = _mapper.Map<BoardColumn>(boardColumnDto);
        _context.BoardColumns.Add(column);
        await _context.SaveChangesAsync();
        
        return Ok(_mapper.Map<BoardColumnDto>(column));
    }

    [HttpPut]
    [Route("{id:guid}")]
    public async Task<ActionResult> Update([FromRoute] Guid id, [FromBody] UpdateBoardColumnRequestDto boardColumnDto)
    {
        // TODO: only for boards in accessible teams, for admins/redactors
        // TODO: check for valid status, there can't be two columns with the same status
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var column = await _context.BoardColumns.FirstOrDefaultAsync(c => c.Id == id);
        if (column == null) return NotFound();
        _mapper.Map(boardColumnDto, column);
        await _context.SaveChangesAsync();
        
        return Ok(_mapper.Map<BoardColumnDto>(column));
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid id)
    {
        // TODO: only for boards in accessible teams, for admins/redactors
        var column = await _context.BoardColumns.FirstOrDefaultAsync(c => c.Id == id);
        if (column == null) return NotFound();
        _context.BoardColumns.Remove(column);

        var columnTasks = await _context.AppTasks.Where(t => t.ColumnId == id).ToListAsync();
        columnTasks.ForEach(task => task.ColumnId = null);
        
        await _context.SaveChangesAsync();
        
        return NoContent();
    }
}