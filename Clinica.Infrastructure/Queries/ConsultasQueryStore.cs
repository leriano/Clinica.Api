using System.Data;
using Clinica.Application.DTOs;
using Clinica.Application.Queries;
using Clinica.Infrastructure.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace Clinica.Infrastructure.Queries;

public class ConsultasQueryStore : IConsultasQueryStore
{
    private readonly AppDbContext _db;
    public ConsultasQueryStore(AppDbContext db) => _db = db;

    public async Task<ConsultaPessoaViewDto?> GetPessoaViewAsync(int id, CancellationToken ct = default)
    {
        var sql = @"
SELECT b.Nome, b.Email, b.Telefone, a.DataConsulta, a.Descricao
FROM clinica.consulta a
JOIN clinica.pessoa  b ON b.Id = a.PessoaId
WHERE a.Id = @Id;";
        using var conn = _db.Database.GetDbConnection();
        if (conn.State != ConnectionState.Open) await conn.OpenAsync(ct);
        return await conn.QuerySingleOrDefaultAsync<ConsultaPessoaViewDto>(sql, new { Id = id });
    }

    // NOVO: lista tudo ou filtra por pessoa
    public async Task<List<ConsultaPessoaViewDto>> ListPessoaViewsAsync(int? pessoaId, CancellationToken ct = default)
    {
        var sql = @"
SELECT b.Nome, b.Email, b.Telefone, a.DataConsulta, a.Descricao
FROM clinica.consulta a
JOIN clinica.pessoa  b ON b.Id = a.PessoaId
WHERE a.Id = @pessoaId
ORDER BY a.DataConsulta;";

        using var conn = _db.Database.GetDbConnection();
        if (conn.State != ConnectionState.Open) await conn.OpenAsync(ct);

        var rows = await conn.QueryAsync<ConsultaPessoaViewDto>(sql, new { PessoaId = pessoaId });
        return rows.ToList();
    }
}
