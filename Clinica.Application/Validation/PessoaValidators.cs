using Clinica.Application.DTOs;
using FluentValidation;

namespace Clinica.Application.Validation;

public class CreatePessoaValidator : AbstractValidator<CreatePessoaRequest>
{
    public CreatePessoaValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().MinimumLength(2);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Telefone)
            .MaximumLength(20)
            .When(x => !string.IsNullOrWhiteSpace(x.Telefone));
    }
}

public class UpdatePessoaValidator : AbstractValidator<UpdatePessoaRequest>
{
    public UpdatePessoaValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().MinimumLength(2);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Telefone)
            .MaximumLength(20)
            .When(x => !string.IsNullOrWhiteSpace(x.Telefone));
    }
}
