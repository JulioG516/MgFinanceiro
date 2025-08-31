using FluentValidation;
using FluentValidation.Results;
using MgFinanceiro.Application.DTOs.Transacao;
using MgFinanceiro.Application.Services;
using MgFinanceiro.Domain.Common;
using MgFinanceiro.Domain.Entities;
using MgFinanceiro.Domain.Interfaces;
using MgFinanceiro.Tests.Extensions;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace MgFinanceiro.Tests.TransacaoTests;

public class TransacaoServiceTests
{
    private readonly Mock<ITransacaoRepository> _transacaoRepositoryMock;
    private readonly Mock<ICategoriaRepository> _categoriaRepositoryMock;
    private readonly Mock<IValidator<CreateTransacaoRequest>> _createValidatorMock;
    private readonly Mock<IValidator<UpdateTransacaoRequest>> _updateValidatorMock;
    private readonly Mock<ILogger<TransacaoService>> _loggerMock;
    private readonly TransacaoService _transacaoService;

    public TransacaoServiceTests()
    {
        _transacaoRepositoryMock = new Mock<ITransacaoRepository>();
        _categoriaRepositoryMock = new Mock<ICategoriaRepository>();
        _createValidatorMock = new Mock<IValidator<CreateTransacaoRequest>>();
        _updateValidatorMock = new Mock<IValidator<UpdateTransacaoRequest>>();
        _loggerMock = new Mock<ILogger<TransacaoService>>();
        _transacaoService = new TransacaoService(
            _transacaoRepositoryMock.Object,
            _categoriaRepositoryMock.Object,
            _createValidatorMock.Object,
            _updateValidatorMock.Object,
            _loggerMock.Object);
    }


    [Fact]
    public async Task GetAllTransacoesAsync_NoFilters_ReturnsAllTransacoes()
    {
        // Arrange
        var transacoes = new List<Transacao>
        {
            new()
            {
                Id = 1,
                Descricao = "Transação 1",
                Valor = 100.50m,
                Data = new DateTime(2025, 8, 1),
                CategoriaId = 1,
                Observacoes = "Obs 1",
                DataCriacao = DateTime.UtcNow,
                Categoria = new Categoria { Id = 1, Nome = "Vendas", Tipo = TipoCategoria.Receita, Ativo = true }
            },
            new()
            {
                Id = 2,
                Descricao = "Transação 2",
                Valor = 200.75m,
                Data = new DateTime(2025, 8, 2),
                CategoriaId = 2,
                Observacoes = "Obs 2",
                DataCriacao = DateTime.UtcNow,
                Categoria = new Categoria { Id = 2, Nome = "Despesas", Tipo = TipoCategoria.Despesa, Ativo = true }
            }
        };
        _transacaoRepositoryMock.Setup(r => r.GetAllTransacoesAsync(null, null, null, null))
            .ReturnsAsync(transacoes);

        // Act
        var result = (await _transacaoService.GetAllTransacoesAsync(null, null, null, null)).ToList();

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(2);
        result.ShouldContain(t =>
            t.Descricao == "Transação 1" && t.Valor == 100.50m && t.CategoriaNome == "Vendas" &&
            t.CategoriaTipo == "Receita");
        result.ShouldContain(t =>
            t.Descricao == "Transação 2" && t.Valor == 200.75m && t.CategoriaNome == "Despesas" &&
            t.CategoriaTipo == "Despesa");
        _transacaoRepositoryMock.Verify(r => r.GetAllTransacoesAsync(null, null, null, null), Times.Once());
    }

    [Fact]
    public async Task GetAllTransacoesAsync_WithFilters_ReturnsFilteredTransacoes()
    {
        // Arrange
        var transacoes = new List<Transacao>
        {
            new()
            {
                Id = 1,
                Descricao = "Transação 1",
                Valor = 100.50m,
                Data = new DateTime(2025, 8, 1),
                CategoriaId = 1,
                Observacoes = "Obs 1",
                DataCriacao = DateTime.UtcNow,
                Categoria = new Categoria { Id = 1, Nome = "Vendas", Tipo = TipoCategoria.Receita, Ativo = true }
            }
        };
        var dataInicio = new DateTime(2025, 8, 1);
        var dataFim = new DateTime(2025, 8, 31);
        var categoriaId = 1;
        var tipoCategoria = TipoCategoria.Receita;
        _transacaoRepositoryMock.Setup(r => r.GetAllTransacoesAsync(dataInicio, dataFim, categoriaId, tipoCategoria))
            .ReturnsAsync(transacoes);

        // Act
        var result = (await _transacaoService.GetAllTransacoesAsync(dataInicio, dataFim, categoriaId, tipoCategoria))
            .ToList();

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(1);
        result.First().Descricao.ShouldBe("Transação 1");
        result.First().Valor.ShouldBe(100.50m);
        result.First().CategoriaId.ShouldBe(1);
        result.First().CategoriaNome.ShouldBe("Vendas");
        result.First().CategoriaTipo.ShouldBe("Receita");
        _transacaoRepositoryMock.Verify(r => r.GetAllTransacoesAsync(dataInicio, dataFim, categoriaId, tipoCategoria), Times.Once());
    }

    [Fact]
    public async Task GetAllTransacoesAsync_NoTransacoes_ReturnsEmptyList()
    {
        // Arrange
        _transacaoRepositoryMock.Setup(r => r.GetAllTransacoesAsync(null, null, null, null))
            .ReturnsAsync(new List<Transacao>());

        // Act
        var result = (await _transacaoService.GetAllTransacoesAsync(null, null, null, null)).ToList();

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeEmpty();
        _transacaoRepositoryMock.Verify(r => r.GetAllTransacoesAsync(null, null, null, null), Times.Once());
        _loggerMock.VerifyLog(LogLevel.Information, "Consultando todas as transações. Filtros: Nenhum filtro", Times.Once());
        _loggerMock.VerifyLog(LogLevel.Information, "Consulta de transações realizada com sucesso. Total encontrado: 0, Tempo de execução:", Times.Once());
    }

    [Fact]
    public async Task GetAllTransacoesAsync_ThrowsException_ReturnsFailure()
    {
        // Arrange
        _transacaoRepositoryMock.Setup(r => r.GetAllTransacoesAsync(null, null, null, null))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Should.ThrowAsync<Exception>(async () => await _transacaoService.GetAllTransacoesAsync(null, null, null, null));
        _transacaoRepositoryMock.Verify(r => r.GetAllTransacoesAsync(null, null, null, null), Times.Once());
    }

    #region GetTransacaoByIdAsync

    [Fact]
    public async Task GetTransacaoByIdAsync_ExistingId_ReturnsTransacao()
    {
        // Arrange
        var transacao = new Transacao
        {
            Id = 1,
            Descricao = "Transação 1",
            Valor = 100.50m,
            Data = new DateTime(2025, 8, 1),
            CategoriaId = 1,
            Observacoes = "Obs 1",
            DataCriacao = DateTime.UtcNow,
            Categoria = new Categoria { Id = 1, Nome = "Vendas", Tipo = TipoCategoria.Receita, Ativo = true }
        };
        _transacaoRepositoryMock.Setup(r => r.GetTransacaoByIdAsync(1))
            .ReturnsAsync(transacao);

        // Act
        var result = await _transacaoService.GetTransacaoByIdAsync(1);

        // Assert
        result.ShouldNotBeNull();
        result.Descricao.ShouldBe("Transação 1");
        result.Valor.ShouldBe(100.50m);
        result.CategoriaId.ShouldBe(1);
        result.CategoriaNome.ShouldBe("Vendas");
        result.CategoriaTipo.ShouldBe("Receita");
        _transacaoRepositoryMock.Verify(r => r.GetTransacaoByIdAsync(1), Times.Once());
    }

    [Fact]
    public async Task GetTransacaoByIdAsync_NonExistentId_ReturnsNull()
    {
        // Arrange
        _transacaoRepositoryMock.Setup(r => r.GetTransacaoByIdAsync(999))
            .ReturnsAsync((Transacao?)null);

        // Act
        var result = await _transacaoService.GetTransacaoByIdAsync(999);

        // Assert
        result.ShouldBeNull();
        _transacaoRepositoryMock.Verify(r => r.GetTransacaoByIdAsync(999), Times.Once());
        _loggerMock.VerifyLog(LogLevel.Information, "Consultando transação por ID: 999", Times.Once());
        _loggerMock.VerifyLog(LogLevel.Warning, "Transação não encontrada. ID: 999", Times.Once());
    }

    [Fact]
    public async Task GetTransacaoByIdAsync_ThrowsException_ThrowsException()
    {
        // Arrange
        _transacaoRepositoryMock.Setup(r => r.GetTransacaoByIdAsync(1))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Should.ThrowAsync<Exception>(async () => await _transacaoService.GetTransacaoByIdAsync(1));
        _transacaoRepositoryMock.Verify(r => r.GetTransacaoByIdAsync(1), Times.Once());
    }

    #endregion

    #region CreateTransacaoAsync

    [Fact]
    public async Task CreateTransacaoAsync_ValidRequest_CreatesTransacao()
    {
        // Arrange
        var request = new CreateTransacaoRequest
        {
            Descricao = "Nova Transação",
            Valor = 100.50m,
            Data = new DateTime(2025, 8, 1),
            CategoriaId = 1,
            Observacoes = "Obs"
        };
        var categoria = new Categoria
        {
            Id = 1,
            Nome = "Vendas",
            Tipo = TipoCategoria.Receita,
            Ativo = true,
            DataCriacao = DateTime.UtcNow
        };
        var createdAt = DateTime.UtcNow;
        var generatedTransacao = new Transacao
        {
            Id = 1,
            Descricao = "Nova Transação",
            Valor = 100.50m,
            Data = new DateTime(2025, 8, 1),
            CategoriaId = 1,
            Observacoes = "Obs",
            DataCriacao = createdAt,
            Categoria = categoria
        };
        _createValidatorMock.Setup(v => v.ValidateAsync(request, default))
            .ReturnsAsync(new ValidationResult());
        _categoriaRepositoryMock.Setup(r => r.GetCategoriaByIdAsync(1))
            .ReturnsAsync(categoria);
        _transacaoRepositoryMock.Setup(r => r.CreateTransacaoAsync(It.IsAny<Transacao>()))
            .ReturnsAsync(Result<Transacao>.Success(generatedTransacao));

        // Act
        var result = await _transacaoService.CreateTransacaoAsync(request);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.Id.ShouldBe(1);
        result.Value.Descricao.ShouldBe(request.Descricao);
        result.Value.Valor.ShouldBe(100.50m);
        result.Value.CategoriaId.ShouldBe(1);
        result.Value.CategoriaNome.ShouldBe("Vendas");
        result.Value.CategoriaTipo.ShouldBe("Receita");
        result.Value.DataCriacao.ShouldBe(createdAt);
        result.Value.Observacoes.ShouldBe(request.Observacoes);
        _transacaoRepositoryMock.Verify(r => r.CreateTransacaoAsync(It.Is<Transacao>(t =>
            t.Descricao == request.Descricao &&
            t.Valor == request.Valor &&
            t.Data.Date == request.Data.Date &&
            t.CategoriaId == request.CategoriaId
        )), Times.Once());
        _categoriaRepositoryMock.Verify(r => r.GetCategoriaByIdAsync(1), Times.Once());
        _createValidatorMock.Verify(v => v.ValidateAsync(request, default), Times.Once());
    }

    [Theory]
    [InlineData("", 100.50, "01/08/2025 00:00:00", 1, "A descrição da transação é obrigatória.")]
    [InlineData("Nova Transação", 0, "01/08/2025 00:00:00", 1, "O valor da transação deve ser maior que zero.")]
    [InlineData("Nova Transação", 100.50, "01/08/2025 00:00:00", 0, "O ID da categoria é obrigatório e deve ser válido.")]
    public async Task CreateTransacaoAsync_InvalidRequest_ReturnsValidationFailure(
        string descricao, decimal valor, string data, int categoriaId, string expectedError)
    {
        // Arrange
        var request = new CreateTransacaoRequest
        {
            Descricao = descricao,
            Valor = valor,
            Data = DateTime.Parse(data),
            CategoriaId = categoriaId,
            Observacoes = "Obs"
        };
        var validationResult = new ValidationResult(new[]
        {
            new ValidationFailure("Field", expectedError)
        });
        _createValidatorMock.Setup(v => v.ValidateAsync(request, default))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _transacaoService.CreateTransacaoAsync(request);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldContain(expectedError);
        _categoriaRepositoryMock.Verify(r => r.GetCategoriaByIdAsync(categoriaId), Times.Never());
        _transacaoRepositoryMock.Verify(r => r.CreateTransacaoAsync(It.IsAny<Transacao>()), Times.Never());
        _createValidatorMock.Verify(v => v.ValidateAsync(request, default), Times.Once());
    }

    [Fact]
    public async Task CreateTransacaoAsync_NonExistentOrInactiveCategoria_ReturnsFailure()
    {
        // Arrange
        var request = new CreateTransacaoRequest
        {
            Descricao = "Nova Transação",
            Valor = 100.50m,
            Data = new DateTime(2025, 8, 1),
            CategoriaId = 999,
            Observacoes = "Obs"
        };
        _createValidatorMock.Setup(v => v.ValidateAsync(request, default))
            .ReturnsAsync(new ValidationResult());
        _categoriaRepositoryMock.Setup(r => r.GetCategoriaByIdAsync(999))
            .ReturnsAsync((Categoria?)null);

        // Act
        var result = await _transacaoService.CreateTransacaoAsync(request);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe("A categoria especificada não existe ou não está ativa.");
        _categoriaRepositoryMock.Verify(r => r.GetCategoriaByIdAsync(999), Times.Once());
        _transacaoRepositoryMock.Verify(r => r.CreateTransacaoAsync(It.IsAny<Transacao>()), Times.Never());
        _createValidatorMock.Verify(v => v.ValidateAsync(request, default), Times.Once());
    }

    [Fact]
    public async Task CreateTransacaoAsync_RepositoryThrowsException_ReturnsFailure()
    {
        // Arrange
        var request = new CreateTransacaoRequest
        {
            Descricao = "Nova Transação",
            Valor = 100.50m,
            Data = new DateTime(2025, 8, 1),
            CategoriaId = 1,
            Observacoes = "Obs"
        };
        var categoria = new Categoria
        {
            Id = 1,
            Nome = "Vendas",
            Tipo = TipoCategoria.Receita,
            Ativo = true,
            DataCriacao = DateTime.UtcNow
        };
        _createValidatorMock.Setup(v => v.ValidateAsync(request, default))
            .ReturnsAsync(new ValidationResult());
        _categoriaRepositoryMock.Setup(r => r.GetCategoriaByIdAsync(1))
            .ReturnsAsync(categoria);
        _transacaoRepositoryMock.Setup(r => r.CreateTransacaoAsync(It.IsAny<Transacao>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _transacaoService.CreateTransacaoAsync(request);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe("Erro interno ao criar transação.");
        _categoriaRepositoryMock.Verify(r => r.GetCategoriaByIdAsync(1), Times.Once());
        _transacaoRepositoryMock.Verify(r => r.CreateTransacaoAsync(It.IsAny<Transacao>()), Times.Once());
        _createValidatorMock.Verify(v => v.ValidateAsync(request, default), Times.Once());
    }

    #endregion

    #region UpdateTransacaoAsync

    [Fact]
    public async Task UpdateTransacaoAsync_ValidRequest_UpdatesTransacao()
    {
        // Arrange
        var request = new UpdateTransacaoRequest
        {
            Id = 1,
            Descricao = "Transação Atualizada",
            Valor = 200.75m,
            Data = new DateTime(2025, 8, 1),
            CategoriaId = 1,
            Observacoes = "Obs Atualizada"
        };
        var existingTransacao = new Transacao
        {
            Id = 1,
            Descricao = "Transação Antiga",
            Valor = 100.50m,
            Data = new DateTime(2025, 7, 1),
            CategoriaId = 1,
            Observacoes = "Obs Antiga",
            DataCriacao = DateTime.UtcNow.AddDays(-1)
        };
        var categoria = new Categoria
        {
            Id = 1,
            Nome = "Vendas",
            Tipo = TipoCategoria.Receita,
            Ativo = true,
            DataCriacao = DateTime.UtcNow
        };
        _updateValidatorMock.Setup(v => v.ValidateAsync(request, default))
            .ReturnsAsync(new ValidationResult());
        _transacaoRepositoryMock.Setup(r => r.GetTransacaoByIdAsync(1))
            .ReturnsAsync(existingTransacao);
        _categoriaRepositoryMock.Setup(r => r.GetCategoriaByIdAsync(1))
            .ReturnsAsync(categoria);
        _transacaoRepositoryMock.Setup(r => r.UpdateTransacaoAsync(It.IsAny<Transacao>()))
            .ReturnsAsync(Result.Success());

        // Act
        var result = await _transacaoService.UpdateTransacaoAsync(request);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        _transacaoRepositoryMock.Verify(r => r.UpdateTransacaoAsync(It.Is<Transacao>(t =>
            t.Id == request.Id &&
            t.Descricao == request.Descricao &&
            t.Valor == request.Valor &&
            t.Data == request.Data.ToUniversalTime() &&
            t.CategoriaId == request.CategoriaId &&
            t.Observacoes == request.Observacoes
        )), Times.Once());
        _transacaoRepositoryMock.Verify(r => r.GetTransacaoByIdAsync(1), Times.Once());
        _categoriaRepositoryMock.Verify(r => r.GetCategoriaByIdAsync(1), Times.Once());
        _updateValidatorMock.Verify(v => v.ValidateAsync(request, default), Times.Once());
    }

    [Theory]
    [InlineData(0, "Transação", 100.50, "2025-08-01", 1, "O ID da transação é obrigatório.")]
    [InlineData(1, "", 100.50, "2025-08-01", 1, "A descrição da transação é obrigatória.")]
    [InlineData(1, "Transação", 0, "2025-08-01", 1, "O valor da transação deve ser maior que zero.")]
    [InlineData(1, "Transação", 100.50, "2025-08-28", 0, "O ID da categoria é obrigatório e deve ser válido.")]
    public async Task UpdateTransacaoAsync_InvalidRequest_ReturnsValidationFailure(
        int id, string descricao, decimal valor, string data, int categoriaId, string expectedError)
    {
        // Arrange
        var request = new UpdateTransacaoRequest
        {
            Id = id,
            Descricao = descricao,
            Valor = valor,
            Data = DateTime.Parse(data),
            CategoriaId = categoriaId,
            Observacoes = "Obs"
        };
        var validationResult = new ValidationResult(new[]
        {
            new ValidationFailure("Field", expectedError)
        });
        _updateValidatorMock.Setup(v => v.ValidateAsync(request, default))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _transacaoService.UpdateTransacaoAsync(request);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldContain(expectedError);
        _transacaoRepositoryMock.Verify(r => r.GetTransacaoByIdAsync(It.IsAny<int>()), Times.Never());
        _categoriaRepositoryMock.Verify(r => r.GetCategoriaByIdAsync(categoriaId), Times.Never());
        _transacaoRepositoryMock.Verify(r => r.UpdateTransacaoAsync(It.IsAny<Transacao>()), Times.Never());
        _updateValidatorMock.Verify(v => v.ValidateAsync(request, default), Times.Once());
    }

    [Fact]
    public async Task UpdateTransacaoAsync_NonExistentTransacao_ReturnsFailure()
    {
        // Arrange
        var request = new UpdateTransacaoRequest
        {
            Id = 999,
            Descricao = "Transação Atualizada",
            Valor = 200.75m,
            Data = new DateTime(2025, 8, 1),
            CategoriaId = 1,
            Observacoes = "Obs"
        };
        _updateValidatorMock.Setup(v => v.ValidateAsync(request, default))
            .ReturnsAsync(new ValidationResult());
        _transacaoRepositoryMock.Setup(r => r.GetTransacaoByIdAsync(999))
            .ReturnsAsync((Transacao?)null);

        // Act
        var result = await _transacaoService.UpdateTransacaoAsync(request);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe("Transação não encontrada.");
        _transacaoRepositoryMock.Verify(r => r.GetTransacaoByIdAsync(999), Times.Once());
        _categoriaRepositoryMock.Verify(r => r.GetCategoriaByIdAsync(1), Times.Never());
        _transacaoRepositoryMock.Verify(r => r.UpdateTransacaoAsync(It.IsAny<Transacao>()), Times.Never());
        _updateValidatorMock.Verify(v => v.ValidateAsync(request, default), Times.Once());
    }

    [Fact]
    public async Task UpdateTransacaoAsync_NonExistentOrInactiveCategoria_ReturnsFailure()
    {
        // Arrange
        var request = new UpdateTransacaoRequest
        {
            Id = 1,
            Descricao = "Transação Atualizada",
            Valor = 200.75m,
            Data = new DateTime(2025, 8, 1),
            CategoriaId = 999,
            Observacoes = "Obs"
        };
        var existingTransacao = new Transacao
        {
            Id = 1,
            Descricao = "Transação Antiga",
            Valor = 100.50m,
            Data = new DateTime(2025, 7, 1),
            CategoriaId = 1,
            Observacoes = "Obs Antiga",
            DataCriacao = DateTime.UtcNow.AddDays(-1)
        };
        _updateValidatorMock.Setup(v => v.ValidateAsync(request, default))
            .ReturnsAsync(new ValidationResult());
        _transacaoRepositoryMock.Setup(r => r.GetTransacaoByIdAsync(1))
            .ReturnsAsync(existingTransacao);
        _categoriaRepositoryMock.Setup(r => r.GetCategoriaByIdAsync(999))
            .ReturnsAsync((Categoria?)null);

        // Act
        var result = await _transacaoService.UpdateTransacaoAsync(request);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe("A categoria especificada não existe ou não está ativa.");
        _transacaoRepositoryMock.Verify(r => r.GetTransacaoByIdAsync(1), Times.Once());
        _categoriaRepositoryMock.Verify(r => r.GetCategoriaByIdAsync(999), Times.Once());
        _transacaoRepositoryMock.Verify(r => r.UpdateTransacaoAsync(It.IsAny<Transacao>()), Times.Never());
        _updateValidatorMock.Verify(v => v.ValidateAsync(request, default), Times.Once());
    }

    [Fact]
    public async Task UpdateTransacaoAsync_RepositoryThrowsException_ReturnsFailure()
    {
        // Arrange
        var request = new UpdateTransacaoRequest
        {
            Id = 1,
            Descricao = "Transação Atualizada",
            Valor = 200.75m,
            Data = new DateTime(2025, 8, 1),
            CategoriaId = 1,
            Observacoes = "Obs"
        };
        var existingTransacao = new Transacao
        {
            Id = 1,
            Descricao = "Transação Antiga",
            Valor = 100.50m,
            Data = new DateTime(2025, 7, 1),
            CategoriaId = 1,
            Observacoes = "Obs Antiga",
            DataCriacao = DateTime.UtcNow.AddDays(-1)
        };
        var categoria = new Categoria
        {
            Id = 1,
            Nome = "Vendas",
            Tipo = TipoCategoria.Receita,
            Ativo = true,
            DataCriacao = DateTime.UtcNow
        };
        _updateValidatorMock.Setup(v => v.ValidateAsync(request, default))
            .ReturnsAsync(new ValidationResult());
        _transacaoRepositoryMock.Setup(r => r.GetTransacaoByIdAsync(1))
            .ReturnsAsync(existingTransacao);
        _categoriaRepositoryMock.Setup(r => r.GetCategoriaByIdAsync(1))
            .ReturnsAsync(categoria);
        _transacaoRepositoryMock.Setup(r => r.UpdateTransacaoAsync(It.IsAny<Transacao>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _transacaoService.UpdateTransacaoAsync(request);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe("Erro interno ao atualizar transação.");
        _transacaoRepositoryMock.Verify(r => r.GetTransacaoByIdAsync(1), Times.Once());
        _categoriaRepositoryMock.Verify(r => r.GetCategoriaByIdAsync(1), Times.Once());
        _transacaoRepositoryMock.Verify(r => r.UpdateTransacaoAsync(It.IsAny<Transacao>()), Times.Once());
        _updateValidatorMock.Verify(v => v.ValidateAsync(request, default), Times.Once());
    }

    #endregion

    #region DeleteTransacaoAsync

    [Fact]
    public async Task DeleteTransacaoAsync_ExistingId_DeletesTransacao()
    {
        // Arrange
        var transacao = new Transacao
        {
            Id = 1,
            Descricao = "Transação 1",
            Valor = 100.50m,
            Data = new DateTime(2025, 8, 1),
            CategoriaId = 1,
            Observacoes = "Obs 1",
            DataCriacao = DateTime.UtcNow,
            Categoria = new Categoria { Id = 1, Nome = "Vendas", Tipo = TipoCategoria.Receita, Ativo = true }
        };
        _transacaoRepositoryMock.Setup(r => r.GetTransacaoByIdAsync(1))
            .ReturnsAsync(transacao);
        _transacaoRepositoryMock.Setup(r => r.DeleteTransacaoAsync(1))
            .ReturnsAsync(Result.Success());

        // Act
        var result = await _transacaoService.DeleteTransacaoAsync(1);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        _transacaoRepositoryMock.Verify(r => r.GetTransacaoByIdAsync(1), Times.Once());
        _transacaoRepositoryMock.Verify(r => r.DeleteTransacaoAsync(1), Times.Once());
    }

    [Fact]
    public async Task DeleteTransacaoAsync_NonExistentId_ReturnsFailure()
    {
        // Arrange
        _transacaoRepositoryMock.Setup(r => r.GetTransacaoByIdAsync(999))
            .ReturnsAsync((Transacao?)null);
        _transacaoRepositoryMock.Setup(r => r.DeleteTransacaoAsync(999))
            .ReturnsAsync(Result.Failure("Transação não encontrada."));

        // Act
        var result = await _transacaoService.DeleteTransacaoAsync(999);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe("Transação não encontrada.");
        _transacaoRepositoryMock.Verify(r => r.GetTransacaoByIdAsync(999), Times.Once());
        _transacaoRepositoryMock.Verify(r => r.DeleteTransacaoAsync(999), Times.Once());
    }

    [Fact]
    public async Task DeleteTransacaoAsync_ThrowsException_ReturnsFailure()
    {
        // Arrange
        var transacao = new Transacao
        {
            Id = 1,
            Descricao = "Transação 1",
            Valor = 100.50m,
            Data = new DateTime(2025, 8, 1),
            CategoriaId = 1,
            Observacoes = "Obs 1",
            DataCriacao = DateTime.UtcNow,
            Categoria = new Categoria { Id = 1, Nome = "Vendas", Tipo = TipoCategoria.Receita, Ativo = true }
        };
        _transacaoRepositoryMock.Setup(r => r.GetTransacaoByIdAsync(1))
            .ReturnsAsync(transacao);
        _transacaoRepositoryMock.Setup(r => r.DeleteTransacaoAsync(1))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _transacaoService.DeleteTransacaoAsync(1);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe("Erro interno ao excluir transação.");
        _transacaoRepositoryMock.Verify(r => r.GetTransacaoByIdAsync(1), Times.Once());
        _transacaoRepositoryMock.Verify(r => r.DeleteTransacaoAsync(1), Times.Once());
    }

    #endregion
}