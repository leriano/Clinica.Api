using Clinica.Domain.Entities;
using Clinica.Domain.Repositories;
using Clinica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Clinica.Infrastructure.Repositories;

public class PessoaRepository : IPessoaRepository
{
    private readonly AppDbContext _db;
    public PessoaRepository(AppDbContext db) => _db = db;

    public async Task<Pessoa?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _db.Pessoas.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<List<Pessoa>> ListAsync(CancellationToken ct = default)
        => await _db.Pessoas.AsNoTracking().OrderBy(p => p.Nome).ToListAsync(ct);

    public async Task<Pessoa> AddAsync(Pessoa pessoa, CancellationToken ct = default)
    {
        _db.Pessoas.Add(pessoa);
        await _db.SaveChangesAsync(ct);
        return pessoa;
    }

    public async Task UpdateAsync(Pessoa pessoa, CancellationToken ct = default)
    {
        _db.Pessoas.Update(pessoa);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsByEmailAsync(string email, int? ignoreId = null, CancellationToken ct = default)
    {
        var q = _db.Pessoas.AsNoTracking().Where(p => p.Email == email);
        if (ignoreId.HasValue) q = q.Where(p => p.Id != ignoreId.Value);
        return await q.AnyAsync(ct);
    }
}
