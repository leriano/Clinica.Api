using Clinica.Application.DTOs;

namespace Clinica.Application.Services;

public interface IPessoaService
{
    Task<PessoaDto?> GetAsync(int id, CancellationToken ct = default);
    Task<List<PessoaDto>> ListAsync(CancellationToken ct = default);
    Task<PessoaDto> CreateAsync(CreatePessoaRequest req, CancellationToken ct = default);
    Task UpdateAsync(int id, UpdatePessoaRequest req, CancellationToken ct = default);
}
