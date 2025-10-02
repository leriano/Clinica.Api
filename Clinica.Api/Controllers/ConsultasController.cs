using Clinica.Application.DTOs;
using Clinica.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Clinica.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConsultasController : ControllerBase
{
    private readonly IConsultaService _svc;
    public ConsultasController(IConsultaService svc) => _svc = svc;

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ConsultaDto>> Get(int id, CancellationToken ct)
    {
        var c = await _svc.GetAsync(id, ct);
        return c is null ? NotFound() : Ok(c);
    }

    [HttpGet("pessoa/{pessoaId:int}")]
    public async Task<ActionResult<List<ConsultaDto>>> ListByPessoa(int pessoaId, CancellationToken ct)
        => Ok(await _svc.ListByPessoaAsync(pessoaId, ct));

    [HttpPost]
    public async Task<ActionResult<ConsultaDto>> Create([FromBody] CreateConsultaRequest req, CancellationToken ct)
    {
        var created = await _svc.CreateAsync(req, ct);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateConsultaRequest req, CancellationToken ct)
    {
        await _svc.UpdateAsync(id, req, ct);
        return NoContent();
    }
}
