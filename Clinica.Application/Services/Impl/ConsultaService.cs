using Clinica.Application.DTOs;
using Clinica.Domain.Entities;
using Clinica.Domain.Repositories;

namespace Clinica.Application.Services.Impl;

public class ConsultaService : IConsultaService
{
    private readonly IConsultaRepository _consultas;
    private readonly IPessoaRepository _pessoas;

    public ConsultaService(IConsultaRepository consultas, IPessoaRepository pessoas)
    {
        _consultas = consultas;
        _pessoas = pessoas;
    }

    public async Task<ConsultaDto?> GetAsync(int id, CancellationToken ct = default)
    {
        var c = await _consultas.GetByIdAsync(id, ct);
        return c is null ? null : Map(c);
    }

    public async Task<List<ConsultaDto>> ListByPessoaAsync(int pessoaId, CancellationToken ct = default)
    {
        var list = await _consultas.ListByPessoaAsync(pessoaId, ct);
        return list.Select(Map).ToList();
    }

    public async Task<ConsultaDto> CreateAsync(CreateConsultaRequest req, CancellationToken ct = default)
    {
        // garante pessoa existente
        if (await _pessoas.GetByIdAsync(req.PessoaId, ct) is null)
            throw new KeyNotFoundException("Pessoa não encontrada.");

        // regra dependente de dados: conflito de agenda
        if (await _consultas.HasConflictAsync(req.PessoaId, req.DataConsulta, null, ct))
            throw new InvalidOperationException("Já existe consulta para essa pessoa nesse horário.");

        var entity = new Consulta(req.PessoaId, req.DataConsulta, req.Descricao);
        var saved = await _consultas.AddAsync(entity, ct);
        return Map(saved);
    }

    public async Task UpdateAsync(int id, UpdateConsultaRequest req, CancellationToken ct = default)
    {
        var entity = await _consultas.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException("Consulta não encontrada.");

        if (await _pessoas.GetByIdAsync(req.PessoaId, ct) is null)
            throw new KeyNotFoundException("Pessoa não encontrada.");

        if (await _consultas.HasConflictAsync(req.PessoaId, req.DataConsulta, id, ct))
            throw new InvalidOperationException("Conflito de horário para essa pessoa.");

        entity.Update(req.PessoaId, req.DataConsulta, req.Descricao);
        await _consultas.UpdateAsync(entity, ct);
    }

    private static ConsultaDto Map(Consulta c)
        => new(c.Id, c.PessoaId, c.DataConsulta, c.Descricao);
}
