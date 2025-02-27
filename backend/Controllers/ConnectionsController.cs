using backend.Data;
using backend.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConnectionsController : ControllerBase
{
  [HttpGet]
  public async Task<ActionResult<List<ConnectionDto>>> GetConnections(AppDbContext context)
  {
    var connections = await context.Connections
      .Include(c => c.ConnectionWords)
      .ThenInclude(cw => cw.Word)
      .ToListAsync();

    var connectionDtos = connections.Select(c => new ConnectionDto
    {
      Id = c.Id,
      ConnectionType = c.ConnectionType,
      Words = c.ConnectionWords.Select(cw => new WordDto
      {
        Id = cw.Word.Id,
        Value = cw.Word.Value
      }).ToList()
    }).ToList();

    return Ok(connectionDtos);
  }

  // This endpoint needs work. It is possible to return copies of the same word!
  [HttpGet("random")]
  public async Task<ActionResult<List<ConnectionDto>>> GetRandomConnections(AppDbContext context)
  {
    var randomConnections = await context.Connections
      .Include(c => c.ConnectionWords)
      .ThenInclude(cw => cw.Word)
      .OrderBy(c => EF.Functions.Random())
      .Take(4)
      .ToListAsync();

    var connectionDtos = randomConnections.Select(c => new ConnectionDto
    {
      Id = c.Id,
      ConnectionType = c.ConnectionType,
      Words = c.ConnectionWords.Select(cw => new WordDto
      {
        Id = cw.Word.Id,
        Value = cw.Word.Value
      }).ToList()
    }).ToList();

    return Ok(connectionDtos);
  }
}
