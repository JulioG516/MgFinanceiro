using FluentValidation;
using MgFinanceiro.Application.DTOs.Transacao;

namespace MgFinanceiro.Application.Validators;

public class CreateTransacaoRequestValidator : AbstractValidator<CreateTransacaoRequest>
{
    public CreateTransacaoRequestValidator()
    {
        RuleFor(t => t.Descricao)
            .NotEmpty().WithMessage("A descrição da transação é obrigatória.")
            .MaximumLength(200).WithMessage("A descrição não pode exceder 200 caracteres.");

        RuleFor(t => t.Valor)
            .GreaterThan(0).WithMessage("O valor da transação deve ser maior que zero.");

        RuleFor(t => t.Data)
            .NotEmpty().WithMessage("A data da transação é obrigatória.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("A data da transação não pode ser futura.");

        RuleFor(t => t.CategoriaId)
            .GreaterThan(0).WithMessage("O ID da categoria é obrigatório e deve ser válido.");

        RuleFor(t => t.Observacoes)
            .MaximumLength(500).WithMessage("As observações não podem exceder 500 caracteres.");
    }
}