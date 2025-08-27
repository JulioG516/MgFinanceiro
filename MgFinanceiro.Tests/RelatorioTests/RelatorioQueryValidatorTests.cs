using FluentValidation.TestHelper;
using MgFinanceiro.Application.DTOs.Relatorio;
using MgFinanceiro.Application.Validators;
using Shouldly;

namespace MgFinanceiro.Tests.RelatorioTests;

public class RelatorioQueryValidatorTests
{
    private readonly RelatorioQueryValidator _validator;

    public RelatorioQueryValidatorTests()
    {
        _validator = new RelatorioQueryValidator();
    }
    
    [Fact]
    public void Should_Have_Error_When_Ano_Is_Less_Than_2000()
    {
        var query = new RelatorioQueryDto { Ano = 1999, Mes = null };

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(q => q.Ano)
            .WithErrorMessage("O ano deve ser maior ou igual a 2000.");
    }
    
    [Fact]
    public void Should_Have_Error_When_Ano_Is_Greater_Than_Current_Year()
    {
        var query = new RelatorioQueryDto { Ano = 2026, Mes = null };

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(q => q.Ano)
            .WithErrorMessage("O ano não pode ser maior que 2025.");
    }
    
    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(13)]
    public void Should_Have_Error_When_Mes_Is_Invalid(int mes)
    {
        var query = new RelatorioQueryDto { Ano = 2025, Mes = mes };

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(q => q.Mes)
            .WithErrorMessage("O mês deve estar entre 1 e 12.");
    }
    
    [Fact]
    public void Should_Have_Error_When_Mes_Is_Specified_And_Ano_Is_Null()
    {
        var query = new RelatorioQueryDto { Ano = null, Mes = 8 };

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(q => q)
            .WithErrorMessage("O ano deve ser informado quando o mês é especificado.");
    }
    
    [Fact]
    public void Should_Not_Have_Error_When_Ano_And_Mes_Are_Null()
    {
        var query = new RelatorioQueryDto { Ano = null, Mes = null };

        var result = _validator.TestValidate(query);

        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Fact]
    public void Should_Not_Have_Error_When_Ano_Is_Valid_And_Mes_Is_Null()
    {
        var query = new RelatorioQueryDto { Ano = 2025, Mes = null };

        var result = _validator.TestValidate(query);

        result.ShouldNotHaveAnyValidationErrors();
    }
    [Fact]
    public void Should_Not_Have_Error_When_Ano_And_Mes_Are_Valid()
    {
        var query = new RelatorioQueryDto { Ano = 2025, Mes = 8 };

        var result = _validator.TestValidate(query);

        result.ShouldNotHaveAnyValidationErrors();
    }
    [Fact]
    public void Should_Have_Multiple_Errors_When_Ano_And_Mes_Are_Invalid()
    {
        var query = new RelatorioQueryDto { Ano = 1999, Mes = 13 };

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(q => q.Ano)
            .WithErrorMessage("O ano deve ser maior ou igual a 2000.");
        result.ShouldHaveValidationErrorFor(q => q.Mes)
            .WithErrorMessage("O mês deve estar entre 1 e 12.");
        result.Errors.ShouldNotBeEmpty();
        result.Errors.Count.ShouldBe(2);
    }

}