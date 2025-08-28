using FluentValidation;
using MgFinanceiro.Application.DTOs.Auth;

namespace MgFinanceiro.Application.Validators;

public class UsuarioRegisterRequestValidator : AbstractValidator<UsuarioRegisterRequest>
{
    public UsuarioRegisterRequestValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório")
            .MaximumLength(100).WithMessage("Nome deve ter no máximo 100 caracteres");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email é obrigatório.")
            .EmailAddress().WithMessage("Deve ser fornecido um email válido.")
            .MaximumLength(255).WithMessage("Email deve ter no maximo 255 caracteres");

        RuleFor(x => x.Senha)
            .MinimumLength(6).WithMessage("Senha deve ter pelo menos 6 caracteres")
            .MaximumLength(100).WithMessage("Senha deve ter no máximo 100 caracteres");
    }
}