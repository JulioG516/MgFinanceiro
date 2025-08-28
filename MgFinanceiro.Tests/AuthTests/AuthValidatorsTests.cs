using FluentValidation.TestHelper;
using MgFinanceiro.Application.DTOs.Auth;
using MgFinanceiro.Application.Validators;

namespace MgFinanceiro.Tests.AuthTests;

public class AuthValidatorsTests
{
    private readonly LoginRequestValidator _loginRequestValidator;
    private readonly UsuarioRegisterRequestValidator _usuarioRegisterRequestValidator;

    public AuthValidatorsTests()
    {
        _loginRequestValidator = new LoginRequestValidator();
        _usuarioRegisterRequestValidator = new UsuarioRegisterRequestValidator();
    }


    #region Usuario Register Tests

    [Fact]
    public void UsuarioRegisterRequest_Should_Have_Error_When_Nome_Is_Empty()
    {
        var request = new UsuarioRegisterRequest
        {
            Nome = "",
            Email = "test@example.com",
            Senha = "validpassword"
        };

        var result = _usuarioRegisterRequestValidator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Nome)
            .WithErrorMessage("Nome é obrigatório");
    }

    [Fact]
    public void UsuarioRegisterRequest_Should_Have_Error_When_Nome_Exceeds_Max_Length()
    {
        var request = new UsuarioRegisterRequest
        {
            Nome = new string('a', 101),
            Email = "test@example.com",
            Senha = "validpassword"
        };

        var result = _usuarioRegisterRequestValidator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Nome)
            .WithErrorMessage("Nome deve ter no máximo 100 caracteres");
    }

    [Fact]
    public void UsuarioRegisterRequest_Should_Have_Error_When_Email_Is_Empty()
    {
        var request = new UsuarioRegisterRequest
        {
            Nome = "Test User",
            Email = "",
            Senha = "validpassword"
        };

        var result = _usuarioRegisterRequestValidator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Email é obrigatório.");
    }

    [Fact]
    public void UsuarioRegisterRequest_Should_Have_Error_When_Email_Is_Invalid()
    {
        var request = new UsuarioRegisterRequest
        {
            Nome = "Test User",
            Email = "invalid-email",
            Senha = "validpassword"
        };

        var result = _usuarioRegisterRequestValidator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Deve ser fornecido um email válido.");
    }

    [Fact]
    public void UsuarioRegisterRequest_Should_Have_Error_When_Email_Exceeds_Max_Length()
    {
        var request = new UsuarioRegisterRequest
        {
            Nome = "Test User",
            Email = "a" + new string('b', 254) + "@example.com",
            Senha = "validpassword"
        };

        var result = _usuarioRegisterRequestValidator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Email deve ter no maximo 255 caracteres");
    }

    [Fact]
    public void UsuarioRegisterRequest_Should_Have_Error_When_Senha_Is_Too_Short()
    {
        var request = new UsuarioRegisterRequest
        {
            Nome = "Test User",
            Email = "test@example.com",
            Senha = "12345"
        };

        var result = _usuarioRegisterRequestValidator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Senha)
            .WithErrorMessage("Senha deve ter pelo menos 6 caracteres");
    }

    [Fact]
    public void UsuarioRegisterRequest_Should_Have_Error_When_Senha_Exceeds_Max_Length()
    {
        var request = new UsuarioRegisterRequest
        {
            Nome = "Test User",
            Email = "test@example.com",
            Senha = new string('a', 101)
        };

        var result = _usuarioRegisterRequestValidator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Senha)
            .WithErrorMessage("Senha deve ter no máximo 100 caracteres");
    }

    [Fact]
    public void UsuarioRegisterRequest_Should_Pass_With_Valid_Request()
    {
        var request = new UsuarioRegisterRequest
        {
            Nome = "Test User",
            Email = "test@example.com",
            Senha = "validpassword"
        };

        var result = _usuarioRegisterRequestValidator.TestValidate(request);
        result.ShouldNotHaveAnyValidationErrors();
    }

    #endregion

    #region Login Request Tests

    [Fact]
    public void LoginRequest_Should_Have_Error_When_Email_Is_Empty()
    {
        var request = new LoginRequest
        {
            Email = "",
            Senha = "validpassword"
        };

        var result = _loginRequestValidator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Email é obrigatório");
    }

    [Fact]
    public void LoginRequest_Should_Have_Error_When_Email_Is_Invalid()
    {
        var request = new LoginRequest
        {
            Email = "invalid-email",
            Senha = "validpassword"
        };

        var result = _loginRequestValidator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Email deve ter formato válido");
    }

    [Fact]
    public void LoginRequest_Should_Have_Error_When_Senha_Is_Empty()
    {
        var request = new LoginRequest
        {
            Email = "test@example.com",
            Senha = ""
        };

        var result = _loginRequestValidator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Senha)
            .WithErrorMessage("Senha é obrigatória");
    }

    [Fact]
    public void LoginRequest_Should_Have_Error_When_Senha_Is_Too_Short()
    {
        var request = new LoginRequest
        {
            Email = "test@example.com",
            Senha = "12345"
        };

        var result = _loginRequestValidator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Senha)
            .WithErrorMessage("Senha deve ter pelo menos 6 caracteres");
    }

    [Fact]
    public void LoginRequest_Should_Have_Error_When_Senha_Exceeds_Max_Length()
    {
        var request = new LoginRequest
        {
            Email = "test@example.com",
            Senha = new string('a', 101)
        };

        var result = _loginRequestValidator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Senha)
            .WithErrorMessage("Senha deve ter no máximo 100 caracteres");
    }

    [Fact]
    public void LoginRequest_Should_Pass_With_Valid_Request()
    {
        var request = new LoginRequest
        {
            Email = "test@example.com",
            Senha = "validpassword"
        };

        var result = _loginRequestValidator.TestValidate(request);
        result.ShouldNotHaveAnyValidationErrors();
    }

    #endregion
}