using FluentValidation;
using MgFinanceiro.Application.DTOs;

namespace MgFinanceiro.Application.Validators;

public class CreateCategoriaRequestValidator : AbstractValidator<CreateCategoriaRequest>
{
    public CreateCategoriaRequestValidator()
    {
        RuleFor(c => c.Nome)
            .NotEmpty().WithMessage("O nome da categoria é obrigatório.")
            .MaximumLength(100).WithMessage("O nome da categoria não pode exceder 100 caracteres.");

        RuleFor(x => x.Tipo)
            .Must(tipo => tipo.Equals("Receita", StringComparison.OrdinalIgnoreCase) ||
                          tipo.Equals("Despesa", StringComparison.OrdinalIgnoreCase))
            .WithMessage("Tipo deve ser 'Receita' ou 'Despesa'");
    }
}