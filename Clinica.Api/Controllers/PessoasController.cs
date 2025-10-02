using Clinica.Application.DTOs;
using Clinica.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Clinica.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PessoasController : ControllerBase
{
    private readonly IPessoaService _svc;
    public PessoasController(IPessoaService svc) => _svc = svc;

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PessoaDto>> Get(int id, CancellationToken ct)
    {
        var p = await _svc.GetAsync(id, ct);
        return p is null ? NotFound() : Ok(p);
    }

    [HttpGet]
    public async Task<ActionResult<List<PessoaDto>>> List(CancellationToken ct)
        => Ok(await _svc.ListAsync(ct));

    [HttpPost]
    public async Task<ActionResult<PessoaDto>> Create([FromBody] CreatePessoaRequest req, CancellationToken ct)
    {
        var created = await _svc.CreateAsync(req, ct);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePessoaRequest req, CancellationToken ct)
    {
        await _svc.UpdateAsync(id, req, ct);
        return NoContent();
    }
}
