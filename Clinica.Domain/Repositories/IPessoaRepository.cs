using Clinica.Domain.Entities;

namespace Clinica.Domain.Repositories;

public interface IPessoaRepository
{
    Task<Pessoa?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<List<Pessoa>> ListAsync(CancellationToken ct = default);
    Task<Pessoa> AddAsync(Pessoa pessoa, CancellationToken ct = default);
    Task UpdateAsync(Pessoa pessoa, CancellationToken ct = default);

    // Suporta regra de negócio (e-mail único)
    Task<bool> ExistsByEmailAsync(string email, int? ignoreId = null, CancellationToken ct = default);
}
