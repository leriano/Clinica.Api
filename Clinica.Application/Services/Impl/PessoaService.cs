using Clinica.Application.DTOs;
using Clinica.Domain.Entities;
using Clinica.Domain.Repositories;

namespace Clinica.Application.Services.Impl;

public class PessoaService : IPessoaService
{
    private readonly IPessoaRepository _repo;

    public PessoaService(IPessoaRepository repo) => _repo = repo;

    public async Task<PessoaDto?> GetAsync(int id, CancellationToken ct = default)
    {
        var p = await _repo.GetByIdAsync(id, ct);
        return p is null ? null : Map(p);
    }

    public async Task<List<PessoaDto>> ListAsync(CancellationToken ct = default)
    {
        var list = await _repo.ListAsync(ct);
        return list.Select(Map).ToList();
    }

    public async Task<PessoaDto> CreateAsync(CreatePessoaRequest req, CancellationToken ct = default)
    {
        // regra de negócio dependente de dados (unicidade)
        if (await _repo.ExistsByEmailAsync(req.Email, null, ct))
            throw new InvalidOperationException("E-mail já cadastrado.");

        var entity = new Pessoa(req.Nome, req.Email, req.Telefone);
        var saved = await _repo.AddAsync(entity, ct);
        return Map(saved);
    }

    public async Task UpdateAsync(int id, UpdatePessoaRequest req, CancellationToken ct = default)
    {
        var entity = await _repo.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException("Pessoa não encontrada.");

        if (await _repo.ExistsByEmailAsync(req.Email, id, ct))
            throw new InvalidOperationException("E-mail já cadastrado por outro usuário.");

        entity.Update(req.Nome, req.Email, req.Telefone);
        await _repo.UpdateAsync(entity, ct);
    }

    private static PessoaDto Map(Pessoa p)
        => new(p.Id, p.Nome, p.Email, p.Telefone);
}
