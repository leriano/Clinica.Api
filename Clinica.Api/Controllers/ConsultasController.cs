using Clinica.Application.DTOs;
using Clinica.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Clinica.Application.Queries;
using Clinica.Domain.Entities;


namespace Clinica.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConsultasController : ControllerBase
{
    private readonly IConsultaService _svc;
    public ConsultasController(IConsultaService svc) => _svc = svc;

    [HttpGet("v2/{pessoaId:int}")]
    public async Task<ActionResult<ConsultaPessoaViewDto>> GetConsultaViewV2(
        [FromRoute] int pessoaId,
        [FromServices] IConsultasQueryStore qs,
        CancellationToken ct)
    {
        var dto = await qs.ListPessoaViewsAsync(pessoaId, ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpGet("v1/{pessoaId:int}")]
    public async Task<ActionResult<List<ConsultaDto>>> ListByPessoaV1(
        [FromRoute] int pessoaId,
        CancellationToken ct)
    {
        var list = await _svc.ListByPessoaAsync(pessoaId, ct);
        return Ok(list);
    }


    [HttpPost]
    public async Task<ActionResult<ConsultaDto>> Create([FromBody] CreateConsultaRequest req, CancellationToken ct)
    {
        var created = await _svc.CreateAsync(req, ct);
        return CreatedAtAction(nameof(GetConsultaViewV2), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateConsultaRequest req, CancellationToken ct)
    {
        await _svc.UpdateAsync(id, req, ct);
        return NoContent();
    }
}
