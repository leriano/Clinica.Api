namespace Clinica.Domain.Entities;

public class Consulta
{
    public int Id { get; private set; }
    public int PessoaId { get; private set; }
    public DateTime DataConsulta { get; private set; }
    public string? Descricao { get; private set; }

    public Pessoa? Pessoa { get; private set; } // navegação opcional

    public Consulta(int pessoaId, DateTime dataConsulta, string? descricao)
    {
        Apply(pessoaId, dataConsulta, descricao);
        Validate();
    }

    private Consulta() { }

    public void Update(int pessoaId, DateTime dataConsulta, string? descricao)
    {
        Apply(pessoaId, dataConsulta, descricao);
        Validate();
    }

    private void Apply(int pessoaId, DateTime dataConsulta, string? descricao)
    {
        PessoaId = pessoaId;
        DataConsulta = DateTime.SpecifyKind(dataConsulta, DateTimeKind.Local);
        Descricao = string.IsNullOrWhiteSpace(descricao) ? null : descricao.Trim();
    }

    private void Validate()
    {
        if (PessoaId <= 0)
            throw new DomainException("PessoaId inválido.");
        if (DataConsulta == DateTime.MinValue)
            throw new DomainException("Data da consulta inválida.");
        // Exemplo de regra opcional:
         if (DataConsulta < DateTime.Now) throw new DomainException("Consulta no passado não é permitida.");
    }
}
