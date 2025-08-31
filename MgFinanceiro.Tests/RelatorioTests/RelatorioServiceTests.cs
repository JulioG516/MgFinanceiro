using FluentValidation;
using MgFinanceiro.Application.DTOs.Relatorio;
using MgFinanceiro.Application.Services;
using MgFinanceiro.Domain.Entities;
using MgFinanceiro.Domain.Interfaces;
using MgFinanceiro.Tests.Extensions;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace MgFinanceiro.Tests.RelatorioTests;

public class RelatorioServiceTests
{
    private readonly Mock<IRelatorioRepository> _relatorioRepositoryMock;
    private readonly Mock<IValidator<RelatorioQueryDto>> _queryValidatorMock;
    private readonly Mock<ILogger<RelatorioService>> _loggerMock;
    private readonly RelatorioService _relatorioService;

    public RelatorioServiceTests()
    {
        _relatorioRepositoryMock = new Mock<IRelatorioRepository>();
        _queryValidatorMock = new Mock<IValidator<RelatorioQueryDto>>();
        _loggerMock = new Mock<ILogger<RelatorioService>>();
        _relatorioService = new RelatorioService(_relatorioRepositoryMock.Object, _queryValidatorMock.Object, _loggerMock.Object);
    }

    #region GetRelatorioResumoAsync

    [Fact]
    public async Task Should_Return_Resumo_When_Ano_And_Mes_Are_Null()
    {
        var query = new RelatorioQueryDto { Ano = null, Mes = null };
        var resumos = new List<RelatorioResumo>
        {
            new() { Ano = 2025, Mes = 8, SaldoTotal = 500.50m, TotalReceitas = 1000.75m, TotalDespesas = 500.25m }
        };
        _queryValidatorMock.Setup(v => v.ValidateAsync(query, default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _relatorioRepositoryMock.Setup(r => r.GetResumoAnoAtualAsync())
            .ReturnsAsync(resumos);

        var result = await _relatorioService.GetRelatorioResumoAsync(query);

        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();
        var responses = result.Value!.ToList();
        responses.ShouldNotBeEmpty();
        responses.Count.ShouldBe(1);
        responses.ShouldContain(r => r.Ano == 2025 && r.Mes == 8 && r.SaldoTotal == 500.50m);
        _relatorioRepositoryMock.Verify(r => r.GetResumoAnoAtualAsync(), Times.Once());
        _relatorioRepositoryMock.Verify(r => r.GetResumoPorPeriodoAsync(It.IsAny<int>(), It.IsAny<int?>()),
            Times.Never());
        _queryValidatorMock.Verify(v => v.ValidateAsync(query, default), Times.Once());
    }

    [Fact]
    public async Task Should_Return_Resumo_When_Ano_Is_Valid_And_Mes_Is_Null()
    {
        var query = new RelatorioQueryDto { Ano = 2025, Mes = null };
        var resumos = new List<RelatorioResumo>
        {
            new() { Ano = 2025, Mes = 8, SaldoTotal = 500.50m, TotalReceitas = 1000.75m, TotalDespesas = 500.25m }
        };
        _queryValidatorMock.Setup(v => v.ValidateAsync(query, default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _relatorioRepositoryMock.Setup(r => r.GetResumoPorPeriodoAsync(2025, null))
            .ReturnsAsync(resumos);

        var result = await _relatorioService.GetRelatorioResumoAsync(query);

        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();
        var responses = result.Value!.ToList();
        responses.ShouldNotBeEmpty();
        responses.Count.ShouldBe(1);
        responses.ShouldContain(r => r.Ano == 2025 && r.Mes == 8 && r.SaldoTotal == 500.50m);
        _relatorioRepositoryMock.Verify(r => r.GetResumoPorPeriodoAsync(2025, null), Times.Once());
        _relatorioRepositoryMock.Verify(r => r.GetResumoAnoAtualAsync(), Times.Never());
        _queryValidatorMock.Verify(v => v.ValidateAsync(query, default), Times.Once());
    }

    [Fact]
    public async Task Should_Return_Resumo_When_Ano_And_Mes_Are_Valid()
    {
        var query = new RelatorioQueryDto { Ano = 2025, Mes = 8 };
        var resumos = new List<RelatorioResumo>
        {
            new() { Ano = 2025, Mes = 8, SaldoTotal = 500.50m, TotalReceitas = 1000.75m, TotalDespesas = 500.25m }
        };
        _queryValidatorMock.Setup(v => v.ValidateAsync(query, default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _relatorioRepositoryMock.Setup(r => r.GetResumoPorPeriodoAsync(2025, 8))
            .ReturnsAsync(resumos);

        var result = await _relatorioService.GetRelatorioResumoAsync(query);

        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();
        var responses = result.Value!.ToList();
        responses.ShouldNotBeEmpty();
        responses.Count.ShouldBe(1);
        responses.ShouldContain(r => r.Ano == 2025 && r.Mes == 8 && r.SaldoTotal == 500.50m);
        _relatorioRepositoryMock.Verify(r => r.GetResumoPorPeriodoAsync(2025, 8), Times.Once());
        _relatorioRepositoryMock.Verify(r => r.GetResumoAnoAtualAsync(), Times.Never());
        _queryValidatorMock.Verify(v => v.ValidateAsync(query, default), Times.Once());
    }

    [Fact]
    public async Task Should_Have_Error_When_Query_Ano_Is_Invalid()
    {
        var query = new RelatorioQueryDto { Ano = null, Mes = 6 };
        var validationResult = new FluentValidation.Results.ValidationResult(new[]
        {
            new FluentValidation.Results.ValidationFailure("Mes", "O ano deve ser informado quando o mês é especificado.")
        });
        _queryValidatorMock.Setup(v => v.ValidateAsync(query, default))
            .ReturnsAsync(validationResult);

        var result = await _relatorioService.GetRelatorioResumoAsync(query);

        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldContain("O ano deve ser informado quando o mês é especificado.");
        _relatorioRepositoryMock.Verify(r => r.GetResumoAnoAtualAsync(), Times.Never());
        _relatorioRepositoryMock.Verify(r => r.GetResumoPorPeriodoAsync(It.IsAny<int>(), It.IsAny<int?>()), Times.Never());
        _queryValidatorMock.Verify(v => v.ValidateAsync(query, default), Times.Once());
    }
    
    [Fact]
    public async Task Should_Have_Error_When_Query_Mes_Is_Invalid()
    {
        var query = new RelatorioQueryDto { Ano = 2025, Mes = 13 };
        var validationResult = new FluentValidation.Results.ValidationResult(new[]
        {
            new FluentValidation.Results.ValidationFailure("Mes", "O mês deve estar entre 1 e 12.")
        });
        _queryValidatorMock.Setup(v => v.ValidateAsync(query, default))
            .ReturnsAsync(validationResult);

        var result = await _relatorioService.GetRelatorioResumoAsync(query);

        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldContain("O mês deve estar entre 1 e 12.");
        _relatorioRepositoryMock.Verify(r => r.GetResumoAnoAtualAsync(), Times.Never());
        _relatorioRepositoryMock.Verify(r => r.GetResumoPorPeriodoAsync(It.IsAny<int>(), It.IsAny<int?>()), Times.Never());
        _queryValidatorMock.Verify(v => v.ValidateAsync(query, default), Times.Once());
        
        _loggerMock.VerifyLog(LogLevel.Information, "Iniciando geração de relatório resumo. Período: 13/2025", Times.Once());
        _loggerMock.VerifyLog(LogLevel.Warning, "Validação falhou para relatório resumo. Período: 13/2025, Erros: O mês deve estar entre 1 e 12.", Times.Once());
    }

    [Fact]
    public async Task Should_Return_Empty_Resumo_List_When_Repository_Returns_Empty()
    {
        var query = new RelatorioQueryDto { Ano = 2025, Mes = 8 };
        _queryValidatorMock.Setup(v => v.ValidateAsync(query, default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _relatorioRepositoryMock.Setup(r => r.GetResumoPorPeriodoAsync(2025, 8))
            .ReturnsAsync(new List<RelatorioResumo>());

        var result = await _relatorioService.GetRelatorioResumoAsync(query);

        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.ShouldBeEmpty();
        _relatorioRepositoryMock.Verify(r => r.GetResumoPorPeriodoAsync(2025, 8), Times.Once());
        _queryValidatorMock.Verify(v => v.ValidateAsync(query, default), Times.Once());
    }
    
    #endregion

    #region GetRelatorioPorCategoriaAsync

    [Fact]
    public async Task Should_Return_PorCategoria_When_Ano_And_Mes_Are_Null()
    {
        var query = new RelatorioQueryDto { Ano = null, Mes = null };
        var relatorios = new List<RelatorioPorCategoria>
        {
            new()
            {
                Ano = 2025,
                Mes = 8,
                CategoriaId = 1,
                CategoriaNome = "Vendas",
                CategoriaTipo = TipoCategoria.Receita,
                Total = 1000.75m
            }
        };
        _queryValidatorMock.Setup(v => v.ValidateAsync(query, default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _relatorioRepositoryMock.Setup(r => r.GetPorCategoriaAnoAtualAsync())
            .ReturnsAsync(relatorios);

        var result = await _relatorioService.GetRelatorioPorCategoriaAsync(query);

        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();
        var responses = result.Value!.ToList();
        responses.ShouldNotBeEmpty();
        responses.Count.ShouldBe(1);
        responses.ShouldContain(r => r.Ano == 2025 && r.Mes == 8 && r.CategoriaId == 1 && r.CategoriaTipo == "Receita");
        _relatorioRepositoryMock.Verify(r => r.GetPorCategoriaAnoAtualAsync(), Times.Once());
        _relatorioRepositoryMock.Verify(r => r.GetPorCategoriaPorPeriodoAsync(It.IsAny<int>(), It.IsAny<int?>()),
            Times.Never());
        _queryValidatorMock.Verify(v => v.ValidateAsync(query, default), Times.Once());
    }

    [Fact]
    public async Task Should_Return_PorCategoria_When_Ano_Is_Valid_And_Mes_Is_Null()
    {
        var query = new RelatorioQueryDto { Ano = 2025, Mes = null };
        var relatorios = new List<RelatorioPorCategoria>
        {
            new()
            {
                Ano = 2025,
                Mes = 8,
                CategoriaId = 1,
                CategoriaNome = "Vendas",
                CategoriaTipo = TipoCategoria.Receita,
                Total = 1000.75m
            }
        };
        _queryValidatorMock.Setup(v => v.ValidateAsync(query, default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _relatorioRepositoryMock.Setup(r => r.GetPorCategoriaPorPeriodoAsync(2025, null))
            .ReturnsAsync(relatorios);

        var result = await _relatorioService.GetRelatorioPorCategoriaAsync(query);

        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();
        var responses = result.Value!.ToList();
        responses.ShouldNotBeEmpty();
        responses.Count.ShouldBe(1);
        responses.ShouldContain(r => r.Ano == 2025 && r.Mes == 8 && r.CategoriaId == 1 && r.CategoriaTipo == "Receita");
        _relatorioRepositoryMock.Verify(r => r.GetPorCategoriaPorPeriodoAsync(2025, null), Times.Once());
        _relatorioRepositoryMock.Verify(r => r.GetPorCategoriaAnoAtualAsync(), Times.Never());
        _queryValidatorMock.Verify(v => v.ValidateAsync(query, default), Times.Once());
    }

    [Fact]
    public async Task Should_Return_PorCategoria_When_Ano_And_Mes_Are_Valid()
    {
        var query = new RelatorioQueryDto { Ano = 2025, Mes = 8 };
        var relatorios = new List<RelatorioPorCategoria>
        {
            new()
            {
                Ano = 2025,
                Mes = 8,
                CategoriaId = 1,
                CategoriaNome = "Vendas",
                CategoriaTipo = TipoCategoria.Receita,
                Total = 1000.75m
            }
        };
        _queryValidatorMock.Setup(v => v.ValidateAsync(query, default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _relatorioRepositoryMock.Setup(r => r.GetPorCategoriaPorPeriodoAsync(2025, 8))
            .ReturnsAsync(relatorios);

        var result = await _relatorioService.GetRelatorioPorCategoriaAsync(query);

        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();
        var responses = result.Value!.ToList();
        responses.ShouldNotBeEmpty();
        responses.Count.ShouldBe(1);
        responses.ShouldContain(r => r.Ano == 2025 && r.Mes == 8 && r.CategoriaId == 1 && r.CategoriaTipo == "Receita");
        _relatorioRepositoryMock.Verify(r => r.GetPorCategoriaPorPeriodoAsync(2025, 8), Times.Once());
        _relatorioRepositoryMock.Verify(r => r.GetPorCategoriaAnoAtualAsync(), Times.Never());
        _queryValidatorMock.Verify(v => v.ValidateAsync(query, default), Times.Once());
    }

    [Fact]
    public async Task Should_Have_Error_When_PorCategoria_Query_Ano_Is_Invalid()
    {
        var query = new RelatorioQueryDto { Ano = null, Mes = 12 };
        var validationResult = new FluentValidation.Results.ValidationResult(new[]
        {
            new FluentValidation.Results.ValidationFailure("Ano, Mes", "O ano deve ser informado quando o mês é especificado.")
        });
        _queryValidatorMock.Setup(v => v.ValidateAsync(query, default))
            .ReturnsAsync(validationResult);

        var result = await _relatorioService.GetRelatorioPorCategoriaAsync(query);
        
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldContain("O ano deve ser informado quando o mês é especificado.");
        _relatorioRepositoryMock.Verify(r => r.GetPorCategoriaAnoAtualAsync(), Times.Never());
        _relatorioRepositoryMock.Verify(r => r.GetPorCategoriaPorPeriodoAsync(It.IsAny<int>(), It.IsAny<int?>()),
            Times.Never());
        _queryValidatorMock.Verify(v => v.ValidateAsync(query, default), Times.Once());
    }

    [Fact]
    public async Task Should_Have_Error_When_PorCategoria_Query_Mes_Is_Invalid()
    {
        var query = new RelatorioQueryDto { Ano = 2025, Mes = 13 };
        var validationResult = new FluentValidation.Results.ValidationResult(new[]
        {
            new FluentValidation.Results.ValidationFailure("Mes", "O mês deve estar entre 1 e 12.")
        });
        _queryValidatorMock.Setup(v => v.ValidateAsync(query, default))
            .ReturnsAsync(validationResult);

        var result = await _relatorioService.GetRelatorioPorCategoriaAsync(query);

        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldContain("O mês deve estar entre 1 e 12.");
        _relatorioRepositoryMock.Verify(r => r.GetPorCategoriaAnoAtualAsync(), Times.Never());
        _relatorioRepositoryMock.Verify(r => r.GetPorCategoriaPorPeriodoAsync(It.IsAny<int>(), It.IsAny<int?>()),
            Times.Never());
        _queryValidatorMock.Verify(v => v.ValidateAsync(query, default), Times.Once());
    }

    [Fact]
    public async Task Should_Return_Empty_PorCategoria_List_When_Repository_Returns_Empty()
    {
        var query = new RelatorioQueryDto { Ano = 2025, Mes = 8 };
        _queryValidatorMock.Setup(v => v.ValidateAsync(query, default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _relatorioRepositoryMock.Setup(r => r.GetPorCategoriaPorPeriodoAsync(2025, 8))
            .ReturnsAsync(new List<RelatorioPorCategoria>());

        var result = await _relatorioService.GetRelatorioPorCategoriaAsync(query);

        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.ShouldBeEmpty();
        _relatorioRepositoryMock.Verify(r => r.GetPorCategoriaPorPeriodoAsync(2025, 8), Times.Once());
        _queryValidatorMock.Verify(v => v.ValidateAsync(query, default), Times.Once());
    }
    #endregion
}