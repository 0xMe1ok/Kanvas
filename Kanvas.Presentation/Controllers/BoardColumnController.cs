using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.DTOs.BoardColumn;
using Presentation.Entities;
using Presentation.Interfaces;

namespace Presentation.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/columns")]
[ApiVersion("1.0")]
public class BoardColumnController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BoardColumnController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    [HttpGet]
    [Route("{id:guid}")]
    public async Task<ActionResult> GetById([FromRoute] Guid id)
    {
        // TODO: only for boards in accessible teams
        var column = await _unitOfWork.Columns.GetByIdAsync(id);
        if (column == null) return NotFound();
        return Ok(_mapper.Map<BoardColumnDto>(column));
    }

    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        // TODO: only for boards in accessible teams
        var columns = await _unitOfWork.Columns.GetAllAsync();
        return Ok(_mapper.Map<List<BoardColumnDto>>(columns));
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateBoardColumnDto boardColumnDto)
    {
        // TODO: only for boards in accessible teams, for admins/redactors
        // TODO: check for valid status, there can't be two columns with the same status
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var column = _mapper.Map<BoardColumn>(boardColumnDto);
        await _unitOfWork.Columns.AddAsync(column);
        await _unitOfWork.CommitAsync();
        
        return Ok(_mapper.Map<BoardColumnDto>(column));
    }

    [HttpPut]
    [Route("{id:guid}")]
    public async Task<ActionResult> Update([FromRoute] Guid id, [FromBody] UpdateBoardColumnDto boardColumnDto)
    {
        // TODO: only for boards in accessible teams, for admins/redactors
        // TODO: check for valid status, there can't be two columns with the same status
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var column = await _unitOfWork.Columns.GetByIdAsync(id);
        if (column == null) return NotFound();
        _mapper.Map(boardColumnDto, column);
        await _unitOfWork.CommitAsync();
        
        return Ok(_mapper.Map<BoardColumnDto>(column));
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid id)
    {
        // TODO: only for boards in accessible teams, for admins/redactors
        var column = await _unitOfWork.Columns.GetByIdAsync(id);
        if (column == null) return NotFound();
        _unitOfWork.Columns.Remove(column);

        await _unitOfWork.Tasks.ClearColumnIdInColumn(id);
        await _unitOfWork.CommitAsync();
        
        return NoContent();
    }
}