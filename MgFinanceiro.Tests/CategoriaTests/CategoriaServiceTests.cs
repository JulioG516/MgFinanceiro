using FluentValidation;
using FluentValidation.Results;
using MgFinanceiro.Application.DTOs.Categoria;
using MgFinanceiro.Application.Services;
using MgFinanceiro.Domain.Common;
using MgFinanceiro.Domain.Entities;
using MgFinanceiro.Domain.Interfaces;
using MgFinanceiro.Tests.Extensions;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace MgFinanceiro.Tests.CategoriaTests;

public class CategoriaServiceTests
{
    private readonly Mock<ICategoriaRepository> _categoriaRepositoryMock;
    private readonly Mock<IValidator<CreateCategoriaRequest>> _createValidatorMock;
    private readonly Mock<ILogger<CategoriaService>> _loggerMock;
    private readonly CategoriaService _categoriaService;

    public CategoriaServiceTests()
    {
        _categoriaRepositoryMock = new Mock<ICategoriaRepository>();
        _createValidatorMock = new Mock<IValidator<CreateCategoriaRequest>>();
        _loggerMock = new Mock<ILogger<CategoriaService>>();
        _categoriaService = new CategoriaService(
            _categoriaRepositoryMock.Object,
            _createValidatorMock.Object,
            _loggerMock.Object);
    }

    #region GetAllCategoriasAsync

    [Fact]
    public async Task GetAllCategoriasAsync_WithoutFilter_ReturnsAllActiveCategorias()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var categorias = new List<Categoria>
        {
            new()
            {
                Id = 1, Nome = "Vendas", Tipo = TipoCategoria.Receita, Ativo = true, DataCriacao = DateTime.UtcNow,
                Transacoes = new List<Transacao>()
            },
            new()
            {
                Id = 2, Nome = "Despesas Operacionais", Tipo = TipoCategoria.Despesa, Ativo = true,
                DataCriacao = DateTime.UtcNow, Transacoes = new List<Transacao>()
            }
        };
        _categoriaRepositoryMock.Setup(r => r.GetAllCategorias(pageNumber, pageSize, null, null))
            .ReturnsAsync((categorias, categorias.Count));

        // Act
        var result = await _categoriaService.GetAllCategoriasAsync(pageNumber, pageSize);

        // Assert
        result.ShouldNotBeNull();
        result.Data.ShouldNotBeNull();
        result.Data.Count().ShouldBe(2);
        result.PageNumber.ShouldBe(pageNumber);
        result.PageSize.ShouldBe(pageSize);
        result.TotalItems.ShouldBe(2);
        result.TotalPages.ShouldBe(1);
        result.Data.ShouldContain(c => c.Nome == "Vendas" && c.Tipo == "Receita" && c.Ativo);
        result.Data.ShouldContain(c => c.Nome == "Despesas Operacionais" && c.Tipo == "Despesa" && c.Ativo);
        _categoriaRepositoryMock.Verify(r => r.GetAllCategorias(pageNumber, pageSize, null, null), Times.Once());
        _loggerMock.VerifyLog(LogLevel.Information,
            $"Consulta de categorias realizada com sucesso. Total encontrado: 2, Total de itens: 2, Página: {pageNumber}, Tamanho da Página: {pageSize}, Tempo de execução:",
            Times.Once());
    }

    [Fact]
    public async Task GetAllCategoriasAsync_WithTipoCategoriaFilter_ReturnsFilteredCategorias()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var categorias = new List<Categoria>
        {
            new()
            {
                Id = 1, Nome = "Vendas", Tipo = TipoCategoria.Receita, Ativo = true, DataCriacao = DateTime.UtcNow,
                Transacoes = new List<Transacao>()
            }
        };
        _categoriaRepositoryMock.Setup(r => r.GetAllCategorias(pageNumber, pageSize, TipoCategoria.Receita, null))
            .ReturnsAsync((categorias, categorias.Count));

        // Act
        var result = await _categoriaService.GetAllCategoriasAsync(pageNumber, pageSize, TipoCategoria.Receita);

        // Assert
        result.ShouldNotBeNull();
        result.Data.ShouldNotBeNull();
        result.Data.Count().ShouldBe(1);
        result.PageNumber.ShouldBe(pageNumber);
        result.PageSize.ShouldBe(pageSize);
        result.TotalItems.ShouldBe(1);
        result.TotalPages.ShouldBe(1);
        result.Data.First().Nome.ShouldBe("Vendas");
        result.Data.First().Tipo.ShouldBe("Receita");
        result.Data.First().Ativo.ShouldBeTrue();
        _categoriaRepositoryMock.Verify(r => r.GetAllCategorias(pageNumber, pageSize, TipoCategoria.Receita, null), Times.Once());
        _loggerMock.VerifyLog(LogLevel.Information,
            $"Consulta de categorias realizada com sucesso. Total encontrado: 1, Total de itens: 1, Página: {pageNumber}, Tamanho da Página: {pageSize}, Tempo de execução:",
            Times.Once());
    }

    [Fact]
    public async Task GetAllCategoriasAsync_NoActiveCategorias_ReturnsEmptyList()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var categorias = new List<Categoria>();
        _categoriaRepositoryMock.Setup(r => r.GetAllCategorias(pageNumber, pageSize, null, null))
            .ReturnsAsync((categorias, categorias.Count));

        // Act
        var result = await _categoriaService.GetAllCategoriasAsync(pageNumber, pageSize);

        // Assert
        result.ShouldNotBeNull();
        result.Data.ShouldNotBeNull();
        result.Data.ShouldBeEmpty();
        result.PageNumber.ShouldBe(pageNumber);
        result.PageSize.ShouldBe(pageSize);
        result.TotalItems.ShouldBe(0);
        result.TotalPages.ShouldBe(0);
        _categoriaRepositoryMock.Verify(r => r.GetAllCategorias(pageNumber, pageSize, null, null), Times.Once());
        _loggerMock.VerifyLog(LogLevel.Information,
            $"Consulta de categorias realizada com sucesso. Total encontrado: 0, Total de itens: 0, Página: {pageNumber}, Tamanho da Página: {pageSize}, Tempo de execução:",
            Times.Once());
    }

    #endregion

    #region CreateCategoriaAsync

    [Fact]
    public async Task CreateCategoriaAsync_ValidRequest_CreatesCategoria()
    {
        // Arrange
        var request = new CreateCategoriaRequest { Nome = "Nova Categoria", Tipo = "Receita" };
        _createValidatorMock.Setup(v => v.ValidateAsync(request, default))
            .ReturnsAsync(new ValidationResult());
        _categoriaRepositoryMock.Setup(r => r.ExistsCategoriaAsync("Nova Categoria", TipoCategoria.Receita))
            .ReturnsAsync(false);
        _categoriaRepositoryMock.Setup(r => r.CreateCategoria(It.IsAny<Categoria>()))
            .ReturnsAsync(Result<Categoria>.Success(new Categoria
            {
                Id = 1, Nome = "Nova Categoria", Tipo = TipoCategoria.Receita, Ativo = true,
                DataCriacao = DateTime.UtcNow
            }));

        // Act
        var result = await _categoriaService.CreateCategoriaAsync(request);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.Id.ShouldBe(1);
        result.Value.Nome.ShouldBe("Nova Categoria");
        result.Value.Tipo.ShouldBe("Receita");
        result.Value.Ativo.ShouldBeTrue();
        _categoriaRepositoryMock.Verify(r => r.ExistsCategoriaAsync("Nova Categoria", TipoCategoria.Receita), Times.Once());
        _categoriaRepositoryMock.Verify(r => r.CreateCategoria(It.Is<Categoria>(c =>
            c.Nome == "Nova Categoria" &&
            c.Tipo == TipoCategoria.Receita &&
            c.Ativo == true &&
            c.DataCriacao != default &&
            c.Transacoes.Count == 0)), Times.Once());
        _createValidatorMock.Verify(v => v.ValidateAsync(request, default), Times.Once());
        _loggerMock.VerifyLog(LogLevel.Information,
            $"Categoria criada com sucesso. ID: 1, Nome: {request.Nome}, Tipo: {request.Tipo}", Times.Once());
    }

    [Fact]
    public async Task CreateCategoriaAsync_InvalidRequest_ReturnsValidationFailure()
    {
        // Arrange
        var request = new CreateCategoriaRequest { Nome = "", Tipo = "Receita" };
        var validationResult = new ValidationResult(new[]
        {
            new ValidationFailure("Nome", "Nome não pode ser vazio.")
        });
        _createValidatorMock.Setup(v => v.ValidateAsync(request, default))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _categoriaService.CreateCategoriaAsync(request);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe("Nome não pode ser vazio.");
        _categoriaRepositoryMock.Verify(r => r.ExistsCategoriaAsync(It.IsAny<string>(), It.IsAny<TipoCategoria>()), Times.Never());
        _categoriaRepositoryMock.Verify(r => r.CreateCategoria(It.IsAny<Categoria>()), Times.Never());
        _createValidatorMock.Verify(v => v.ValidateAsync(request, default), Times.Once());
        _loggerMock.VerifyLog(LogLevel.Warning,
            $"Validação falhou ao criar categoria. Erros: {result.Error}. Dados da requisição:", Times.Once());
    }

    [Fact]
    public async Task CreateCategoriaAsync_DuplicateCategoria_ReturnsFailure()
    {
        // Arrange
        var request = new CreateCategoriaRequest { Nome = "Vendas", Tipo = "Receita" };
        _createValidatorMock.Setup(v => v.ValidateAsync(request, default))
            .ReturnsAsync(new ValidationResult());
        _categoriaRepositoryMock.Setup(r => r.ExistsCategoriaAsync("Vendas", TipoCategoria.Receita))
            .ReturnsAsync(true);

        // Act
        var result = await _categoriaService.CreateCategoriaAsync(request);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe("Já existe uma categoria com este nome e tipo.");
        _categoriaRepositoryMock.Verify(r => r.ExistsCategoriaAsync("Vendas", TipoCategoria.Receita), Times.Once());
        _categoriaRepositoryMock.Verify(r => r.CreateCategoria(It.IsAny<Categoria>()), Times.Never());
        _createValidatorMock.Verify(v => v.ValidateAsync(request, default), Times.Once());
        _loggerMock.VerifyLog(LogLevel.Warning,
            $"Tentativa de criar categoria duplicada. Nome: {request.Nome}, Tipo: {request.Tipo}", Times.Once());
    }

    [Fact]
    public async Task CreateCategoriaAsync_RepositoryFailure_ReturnsFailure()
    {
        // Arrange
        var request = new CreateCategoriaRequest { Nome = "Nova Categoria", Tipo = "Receita" };
        _createValidatorMock.Setup(v => v.ValidateAsync(request, default))
            .ReturnsAsync(new ValidationResult());
        _categoriaRepositoryMock.Setup(r => r.ExistsCategoriaAsync("Nova Categoria", TipoCategoria.Receita))
            .ReturnsAsync(false);
        _categoriaRepositoryMock.Setup(r => r.CreateCategoria(It.IsAny<Categoria>()))
            .ReturnsAsync(Result<Categoria>.Failure("Erro no banco de dados."));

        // Act
        var result = await _categoriaService.CreateCategoriaAsync(request);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe("Erro no banco de dados.");
        _categoriaRepositoryMock.Verify(r => r.ExistsCategoriaAsync("Nova Categoria", TipoCategoria.Receita), Times.Once());
        _categoriaRepositoryMock.Verify(r => r.CreateCategoria(It.IsAny<Categoria>()), Times.Once());
        _createValidatorMock.Verify(v => v.ValidateAsync(request, default), Times.Once());
        _loggerMock.VerifyLog(LogLevel.Error,
            $"Falha ao criar categoria no repositório. Erro: {result.Error}. Dados: Nome={request.Nome}, Tipo={request.Tipo}",
            Times.Once());
    }

    #endregion

    #region UpdateCategoriaStatusAsync

    [Fact]
    public async Task UpdateCategoriaStatusAsync_ValidRequest_UpdatesCategoriaStatus()
    {
        // Arrange
        var request = new UpdateCategoriaStatusRequest { Id = 1, Ativo = false };
        var existingCategoria = new Categoria
        {
            Id = 1,
            Nome = "Vendas",
            Tipo = TipoCategoria.Receita,
            Ativo = true,
            DataCriacao = DateTime.UtcNow,
            Transacoes = new List<Transacao>()
        };
        _categoriaRepositoryMock.Setup(r => r.GetCategoriaByIdAsync(1))
            .ReturnsAsync(existingCategoria);
        _categoriaRepositoryMock.Setup(r => r.UpdateCategoria(It.IsAny<Categoria>()))
            .ReturnsAsync(Result<Categoria>.Success(new Categoria
            {
                Id = 1,
                Nome = "Vendas",
                Tipo = TipoCategoria.Receita,
                Ativo = false,
                DataCriacao = existingCategoria.DataCriacao,
                Transacoes = new List<Transacao>()
            }));

        // Act
        var result = await _categoriaService.UpdateCategoriaStatusAsync(1, request);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.Id.ShouldBe(1);
        result.Value.Nome.ShouldBe("Vendas");
        result.Value.Tipo.ShouldBe("Receita");
        result.Value.Ativo.ShouldBeFalse();
        _categoriaRepositoryMock.Verify(r => r.GetCategoriaByIdAsync(1), Times.Once());
        _categoriaRepositoryMock.Verify(r => r.UpdateCategoria(It.Is<Categoria>(c =>
            c.Id == 1 &&
            c.Ativo == false &&
            c.Nome == "Vendas" &&
            c.Tipo == TipoCategoria.Receita &&
            c.DataCriacao != default)), Times.Once());
        _loggerMock.VerifyLog(LogLevel.Information,
            $"Status da categoria atualizado com sucesso. ID: 1, Nome: Vendas, Status: False", Times.Once());
    }

    [Fact]
    public async Task UpdateCategoriaStatusAsync_IdMismatch_ReturnsFailure()
    {
        // Arrange
        var request = new UpdateCategoriaStatusRequest { Id = 2, Ativo = false };

        // Act
        var result = await _categoriaService.UpdateCategoriaStatusAsync(1, request);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(
            "O identificador fornecido na URL não corresponde ao especificado no corpo da requisição.");
        _categoriaRepositoryMock.Verify(r => r.GetCategoriaByIdAsync(It.IsAny<int>()), Times.Never());
        _categoriaRepositoryMock.Verify(r => r.UpdateCategoria(It.IsAny<Categoria>()), Times.Never());
        _loggerMock.VerifyLog(LogLevel.Warning,
            $"ID da URL não corresponde ao ID do corpo da requisição. URL_ID: 1, Body_ID: 2", Times.Once());
    }

    [Fact]
    public async Task UpdateCategoriaStatusAsync_CategoriaNotFound_ReturnsFailure()
    {
        // Arrange
        var request = new UpdateCategoriaStatusRequest { Id = 1, Ativo = false };
        _categoriaRepositoryMock.Setup(r => r.GetCategoriaByIdAsync(1))
            .ReturnsAsync((Categoria?)null);

        // Act
        var result = await _categoriaService.UpdateCategoriaStatusAsync(1, request);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe("Categoria não encontrada.");
        _categoriaRepositoryMock.Verify(r => r.GetCategoriaByIdAsync(1), Times.Once());
        _categoriaRepositoryMock.Verify(r => r.UpdateCategoria(It.IsAny<Categoria>()), Times.Never());
        _loggerMock.VerifyLog(LogLevel.Warning, $"Categoria não encontrada para atualização de status. ID: 1",
            Times.Once());
    }

    [Fact]
    public async Task UpdateCategoriaStatusAsync_RepositoryFailure_ReturnsFailure()
    {
        // Arrange
        var request = new UpdateCategoriaStatusRequest { Id = 1, Ativo = false };
        var existingCategoria = new Categoria
        {
            Id = 1,
            Nome = "Vendas",
            Tipo = TipoCategoria.Receita,
            Ativo = true,
            DataCriacao = DateTime.UtcNow,
            Transacoes = new List<Transacao>()
        };
        _categoriaRepositoryMock.Setup(r => r.GetCategoriaByIdAsync(1))
            .ReturnsAsync(existingCategoria);
        _categoriaRepositoryMock.Setup(r => r.UpdateCategoria(It.IsAny<Categoria>()))
            .ReturnsAsync(Result<Categoria>.Failure("Erro ao atualizar no banco de dados."));

        // Act
        var result = await _categoriaService.UpdateCategoriaStatusAsync(1, request);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe("Erro ao atualizar no banco de dados.");
        _categoriaRepositoryMock.Verify(r => r.GetCategoriaByIdAsync(1), Times.Once());
        _categoriaRepositoryMock.Verify(r => r.UpdateCategoria(It.Is<Categoria>(c =>
            c.Id == 1 && c.Ativo == false)), Times.Once());
        _loggerMock.VerifyLog(LogLevel.Error,
            $"Falha ao atualizar status da categoria no repositório. ID: 1, Erro: {result.Error}", Times.Once());
    }

    #endregion

    #region GetCategoriaByIdAsync

    [Fact]
    public async Task GetCategoriaByIdAsync_ValidId_ReturnsCategoria()
    {
        // Arrange
        var categoria = new Categoria
        {
            Id = 1,
            Nome = "Vendas",
            Tipo = TipoCategoria.Receita,
            Ativo = true,
            DataCriacao = DateTime.UtcNow,
            Transacoes = new List<Transacao>()
        };
        _categoriaRepositoryMock.Setup(r => r.GetCategoriaByIdAsync(1))
            .ReturnsAsync(categoria);

        // Act
        var result = await _categoriaService.GetCategoriaByIdAsync(1);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(1);
        result.Nome.ShouldBe("Vendas");
        result.Tipo.ShouldBe("Receita");
        result.Ativo.ShouldBeTrue();
        _categoriaRepositoryMock.Verify(r => r.GetCategoriaByIdAsync(1), Times.Once());
        _loggerMock.VerifyLog(LogLevel.Information,
            $"Categoria encontrada com sucesso. ID: 1, Nome: Vendas, Tipo: Receita", Times.Once());
    }

    [Fact]
    public async Task GetCategoriaByIdAsync_InvalidId_ReturnsNull()
    {
        // Arrange
        _categoriaRepositoryMock.Setup(r => r.GetCategoriaByIdAsync(1))
            .ReturnsAsync((Categoria?)null);

        // Act
        var result = await _categoriaService.GetCategoriaByIdAsync(1);

        // Assert
        result.ShouldBeNull();
        _categoriaRepositoryMock.Verify(r => r.GetCategoriaByIdAsync(1), Times.Once());
        _loggerMock.VerifyLog(LogLevel.Warning, $"Categoria não encontrada. ID: 1", Times.Once());
    }

    #endregion
}