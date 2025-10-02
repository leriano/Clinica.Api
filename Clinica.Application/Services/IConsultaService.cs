using Clinica.Application.DTOs;

namespace Clinica.Application.Services;

public interface IConsultaService
{
    Task<ConsultaDto?> GetAsync(int id, CancellationToken ct = default);
    Task<List<ConsultaDto>> ListByPessoaAsync(int pessoaId, CancellationToken ct = default);
    Task<ConsultaDto> CreateAsync(CreateConsultaRequest req, CancellationToken ct = default);
    Task UpdateAsync(int id, UpdateConsultaRequest req, CancellationToken ct = default);
}
