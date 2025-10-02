namespace Clinica.Domain.Entities;

public class Pessoa
{
    public int Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string? Telefone { get; private set; }

    // ctor de criação: força passar por regras já na criação
    public Pessoa(string nome, string email, string? telefone)
    {
        Apply(nome, email, telefone);
        Validate();
    }

    // para EF
    private Pessoa() { }

    // único ponto de mutação “rico”
    public void Update(string nome, string email, string? telefone)
    {
        Apply(nome, email, telefone);
        Validate();
    }

    private void Apply(string nome, string email, string? telefone)
    {
        Nome = nome?.Trim() ?? string.Empty;
        Email = email?.Trim() ?? string.Empty;
        Telefone = string.IsNullOrWhiteSpace(telefone) ? null : telefone.Trim();
    }

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Nome) || Nome.Length < 2)
            throw new DomainException("Nome inválido.");
        if (string.IsNullOrWhiteSpace(Email) || !Email.Contains('@'))
            throw new DomainException("E-mail inválido.");
    }
}
