using Clinica.Application.DTOs;
using FluentValidation;

namespace Clinica.Application.Validation;

public class CreateConsultaValidator : AbstractValidator<CreateConsultaRequest>
{
    public CreateConsultaValidator()
    {
        RuleFor(x => x.PessoaId).GreaterThan(0);
        RuleFor(x => x.DataConsulta).GreaterThan(DateTime.MinValue);
        // Regra opcional: garantir futuro
        // RuleFor(x => x.DataConsulta).Must(d => d > DateTime.Now).WithMessage("A data deve ser futura.");
    }
}

public class UpdateConsultaValidator : AbstractValidator<UpdateConsultaRequest>
{
    public UpdateConsultaValidator()
    {
        RuleFor(x => x.PessoaId).GreaterThan(0);
        RuleFor(x => x.DataConsulta).GreaterThan(DateTime.MinValue);
    }
}
