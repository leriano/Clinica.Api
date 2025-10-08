using Clinica.Application.DTOs;

namespace Clinica.Application.Queries;

public interface IConsultasQueryStore
{
    Task<ConsultaPessoaViewDto?> GetPessoaViewAsync(int id, CancellationToken ct = default);
    Task<List<ConsultaPessoaViewDto>> ListPessoaViewsAsync(int? pessoaId, CancellationToken ct = default);

}
