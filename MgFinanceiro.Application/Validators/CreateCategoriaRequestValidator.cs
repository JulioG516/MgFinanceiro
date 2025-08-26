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

        RuleFor(c => c.Tipo)
            .IsInEnum().WithMessage("O tipo de categoria deve ser válido (Receita ou Despesa).");
    }
}