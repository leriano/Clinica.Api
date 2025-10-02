namespace Clinica.Application.DTOs;

public record PessoaDto(int Id, string Nome, string Email, string? Telefone);
public record CreatePessoaRequest(string Nome, string Email, string? Telefone);
public record UpdatePessoaRequest(string Nome, string Email, string? Telefone);
