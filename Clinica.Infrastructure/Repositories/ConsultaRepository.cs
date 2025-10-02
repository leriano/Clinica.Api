using Clinica.Domain.Entities;
using Clinica.Domain.Repositories;
using Clinica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Clinica.Infrastructure.Repositories;

public class ConsultaRepository : IConsultaRepository
{
    private readonly AppDbContext _db;
    public ConsultaRepository(AppDbContext db) => _db = db;

    public async Task<Consulta?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _db.Consultas.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task<List<Consulta>> ListByPessoaAsync(int pessoaId, CancellationToken ct = default)
        => await _db.Consultas.AsNoTracking()
            .Where(c => c.PessoaId == pessoaId)
            .OrderBy(c => c.DataConsulta)
            .ToListAsync(ct);

    public async Task<Consulta> AddAsync(Consulta consulta, CancellationToken ct = default)
    {
        _db.Consultas.Add(consulta);
        await _db.SaveChangesAsync(ct);
        return consulta;
    }

    public async Task UpdateAsync(Consulta consulta, CancellationToken ct = default)
    {
        _db.Consultas.Update(consulta);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<bool> HasConflictAsync(int pessoaId, DateTime dataConsulta, int? ignoreId = null, CancellationToken ct = default)
    {
        var q = _db.Consultas.AsNoTracking()
            .Where(c => c.PessoaId == pessoaId && c.DataConsulta == dataConsulta);
        if (ignoreId.HasValue) q = q.Where(c => c.Id != ignoreId.Value);
        return await q.AnyAsync(ct);
    }
}
