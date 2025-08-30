using FluentValidation;
using MgFinanceiro.Application.DTOs.Categoria;

namespace MgFinanceiro.Application.Validators;

public class UpdateCategoriaRequestValidator : AbstractValidator<UpdateCategoriaStatusRequest>
{
    public UpdateCategoriaRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id deve ser fornecido");

        RuleFor(x => x.Ativo)
            .NotNull().WithMessage("Ativo deve ser fornecido");
    }
}