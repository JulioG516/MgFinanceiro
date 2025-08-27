using FluentValidation;
using MgFinanceiro.Application.DTOs.Relatorio;

namespace MgFinanceiro.Application.Validators;

public class RelatorioQueryValidator : AbstractValidator<RelatorioQueryDto>
{
    public RelatorioQueryValidator()
    {
        RuleFor(q => q.Ano)
            .GreaterThanOrEqualTo(2000).When(q => q.Ano.HasValue)
            .WithMessage("O ano deve ser maior ou igual a 2000.")
            .LessThanOrEqualTo(DateTime.UtcNow.Year).When(q => q.Ano.HasValue)
            .WithMessage($"O ano não pode ser maior que {DateTime.UtcNow.Year}.");

        RuleFor(q => q.Mes)
            .InclusiveBetween(1, 12).When(q => q.Mes.HasValue)
            .WithMessage("O mês deve estar entre 1 e 12.");
        
        RuleFor(q => q)
            .Custom((query, context) =>
            {
                if (!query.Ano.HasValue && query.Mes.HasValue)
                {
                    context.AddFailure("O ano deve ser informado quando o mês é especificado.");
                }
            });
    }
}