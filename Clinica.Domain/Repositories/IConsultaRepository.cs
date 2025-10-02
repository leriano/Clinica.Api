using Clinica.Domain.Entities;

namespace Clinica.Domain.Repositories;

public interface IConsultaRepository
{
    Task<Consulta?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<List<Consulta>> ListByPessoaAsync(int pessoaId, CancellationToken ct = default);
    Task<Consulta> AddAsync(Consulta consulta, CancellationToken ct = default);
    Task UpdateAsync(Consulta consulta, CancellationToken ct = default);

    // Suporta regra de negócio (conflito de horário)
    Task<bool> HasConflictAsync(int pessoaId, DateTime dataConsulta, int? ignoreId = null, CancellationToken ct = default);
}
