namespace Clinica.Application.DTOs;

public record ConsultaDto(int Id, int PessoaId, DateTime DataConsulta, string? Descricao);
public record CreateConsultaRequest(int PessoaId, DateTime DataConsulta, string? Descricao);
public record UpdateConsultaRequest(int PessoaId, DateTime DataConsulta, string? Descricao);
public record ConsultaPessoaViewDto(
    string Nome,
    string Email,
    string? Telefone,
    DateTime DataConsulta,
    string? Descricao
);
