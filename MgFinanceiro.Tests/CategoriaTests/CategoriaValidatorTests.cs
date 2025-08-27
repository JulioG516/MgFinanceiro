using FluentValidation.TestHelper;
using MgFinanceiro.Application.DTOs.Categoria;
using MgFinanceiro.Application.Validators;

namespace MgFinanceiro.Tests.CategoriaTests;

public class CategoriaValidatorTests
{

    private readonly CreateCategoriaRequestValidator _createCategoriaRequestValidator;

    public CategoriaValidatorTests()
    {
        _createCategoriaRequestValidator = new CreateCategoriaRequestValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Nome_Its_Empty()
    {
        var t1 = new CreateCategoriaRequest
        {
            Nome = "",
            Tipo = "Receita"
        };

        var result = _createCategoriaRequestValidator.TestValidate(t1);
        result.ShouldHaveValidationErrorFor(x => x.Nome)
            .WithErrorMessage("O nome da categoria é obrigatório.");
    }

    [Fact]
    public void Should_Have_Error_When_Tipo_Its_Not_Receita_Or_Despesa()
    {
        var request = new CreateCategoriaRequest
        {
            Nome = "Nova Categoria",
            Tipo = ""
        };

        var result = _createCategoriaRequestValidator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Tipo)
            .WithErrorMessage("Tipo deve ser 'Receita' ou 'Despesa'");
    }

    [Fact]
    public void Should_Have_Error_With_Nome_Passing_Max_Length()
    {
        var request = new CreateCategoriaRequest
        {
            Nome = new string('a', 101),
            Tipo = "Receita"
        };
        var result = _createCategoriaRequestValidator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Nome)
            .WithErrorMessage("O nome da categoria não pode exceder 100 caracteres.");
    }

    [Fact]
    public void Should_Pass_With_Valid_Request()
    {
        var request = new CreateCategoriaRequest
        {
            Nome = "Nova Categoria",
            Tipo = "Receita"
        };
        
        var result = _createCategoriaRequestValidator.TestValidate(request);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
