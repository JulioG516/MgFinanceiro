using FluentValidation;
using MgFinanceiro.Application.DTOs.Categoria;
using MgFinanceiro.Application.Services;
using MgFinanceiro.Domain.Common;
using MgFinanceiro.Domain.Entities;
using MgFinanceiro.Domain.Interfaces;
using Moq;
using Shouldly;

namespace MgFinanceiro.Tests.CategoriaTests;

public class CategoriaServiceTests
{
    private readonly Mock<ICategoriaRepository> _categoriaRepositoryMock;
    private readonly Mock<IValidator<CreateCategoriaRequest>> _validatorMock;
    private readonly CategoriaService _categoriaService;

    public CategoriaServiceTests()
    {
        _categoriaRepositoryMock = new Mock<ICategoriaRepository>();
        _validatorMock = new Mock<IValidator<CreateCategoriaRequest>>();
        _categoriaService = new CategoriaService(_categoriaRepositoryMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task GetAllCategoriasAsync_WithoutFilter_ReturnsAllActiveCategorias()
    {
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
        _categoriaRepositoryMock.Setup(r => r.GetAllCategorias(null))
            .ReturnsAsync(categorias);

        var result = (await _categoriaService.GetAllCategoriasAsync()).ToList();

        result.ShouldNotBeNull();
        result.Count().ShouldBe(2);
        result.ShouldContain(c => c.Nome == "Vendas" && c.Tipo == "Receita" && c.Ativo);
        result.ShouldContain(c => c.Nome == "Despesas Operacionais" && c.Tipo == "Despesa" && c.Ativo);
        _categoriaRepositoryMock.Verify(r => r.GetAllCategorias(null), Times.Once());
    }


    [Fact]
    public async Task GetAllCategoriasAsync_WithTipoCategoriaFilter_ReturnsFilteredCategorias()
    {
        var categorias = new List<Categoria>
        {
            new()
            {
                Id = 1, Nome = "Vendas", Tipo = TipoCategoria.Receita, Ativo = true, DataCriacao = DateTime.UtcNow,
                Transacoes = new List<Transacao>()
            }
        };
        _categoriaRepositoryMock.Setup(r => r.GetAllCategorias(TipoCategoria.Receita))
            .ReturnsAsync(categorias);

        var result = (await _categoriaService.GetAllCategoriasAsync(TipoCategoria.Receita)).ToList();

        result.ShouldNotBeNull();
        result.Count.ShouldBe(1);
        result.First().Nome.ShouldBe("Vendas");
        result.First().Tipo.ShouldBe("Receita");
        result.First().Ativo.ShouldBeTrue();
        _categoriaRepositoryMock.Verify(r => r.GetAllCategorias(TipoCategoria.Receita), Times.Once());
    }

    [Fact]
    public async Task GetAllCategoriasAsync_NoActiveCategorias_ReturnsEmptyList()
    {
        _categoriaRepositoryMock.Setup(r => r.GetAllCategorias(null))
            .ReturnsAsync(new List<Categoria>());

        var result = (await _categoriaService.GetAllCategoriasAsync()).ToList();

        result.ShouldNotBeNull();
        result.ShouldBeEmpty();
        _categoriaRepositoryMock.Verify(r => r.GetAllCategorias(null), Times.Once());
    }

    [Fact]
    public async Task CreateCategoriaAsync_ValidRequest_CreatesCategoria()
    {
        var request = new CreateCategoriaRequest { Nome = "Nova Categoria", Tipo = "Receita" };
        _validatorMock.Setup(v => v.ValidateAsync(request, default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _categoriaRepositoryMock.Setup(r => r.GetAllCategorias(null))
            .ReturnsAsync(new List<Categoria>());
        _categoriaRepositoryMock.Setup(r => r.CreateCategoria(It.IsAny<Categoria>()))
            .ReturnsAsync(Result.Success());

        var result = await _categoriaService.CreateCategoriaAsync(request);

        result.IsSuccess.ShouldBeTrue();
        _categoriaRepositoryMock.Verify(r => r.CreateCategoria(It.Is<Categoria>(c =>
            c.Nome == "Nova Categoria" &&
            c.Tipo == TipoCategoria.Receita &&
            c.Ativo == true &&
            c.DataCriacao != default &&
            c.Transacoes.Count == 0)), Times.Once());
        _categoriaRepositoryMock.Verify(r => r.GetAllCategorias(null), Times.Once());
        _validatorMock.Verify(v => v.ValidateAsync(request, default), Times.Once());
    }

    [Fact]
    public async Task CreateCategoriaAsync_InvalidRequest_ReturnsValidationFailure()
    {
        var request = new CreateCategoriaRequest { Nome = "", Tipo = "Receita" };
        var validationResult = new FluentValidation.Results.ValidationResult(new[]
        {
            new FluentValidation.Results.ValidationFailure("Nome", "Nome não pode ser vazio.")
        });
        _validatorMock.Setup(v => v.ValidateAsync(request, default))
            .ReturnsAsync(validationResult);

        var result = await _categoriaService.CreateCategoriaAsync(request);

        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe("Nome não pode ser vazio.");
        _categoriaRepositoryMock.Verify(r => r.GetAllCategorias(null), Times.Never());
        _categoriaRepositoryMock.Verify(r => r.CreateCategoria(It.IsAny<Categoria>()), Times.Never());
        _validatorMock.Verify(v => v.ValidateAsync(request, default), Times.Once());
    }

    [Fact]
    public async Task CreateCategoriaAsync_DuplicateCategoria_ReturnsFailure()
    {
        var request = new CreateCategoriaRequest { Nome = "Vendas", Tipo = "Receita" };
        _validatorMock.Setup(v => v.ValidateAsync(request, default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _categoriaRepositoryMock.Setup(r => r.GetAllCategorias(null))
            .ReturnsAsync(new List<Categoria>
            {
                new()
                {
                    Id = 1, Nome = "Vendas", Tipo = TipoCategoria.Receita, Ativo = true, DataCriacao = DateTime.UtcNow,
                    Transacoes = new List<Transacao>()
                }
            });

        var result = await _categoriaService.CreateCategoriaAsync(request);

        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe("Já existe uma categoria ativa com este nome e tipo.");
        _categoriaRepositoryMock.Verify(r => r.GetAllCategorias(null), Times.Once());
        _categoriaRepositoryMock.Verify(r => r.CreateCategoria(It.IsAny<Categoria>()), Times.Never());
        _validatorMock.Verify(v => v.ValidateAsync(request, default), Times.Once());
    }

    [Fact]
    public async Task CreateCategoriaAsync_RepositoryFailure_ReturnsFailure()
    {
        var request = new CreateCategoriaRequest { Nome = "Nova Categoria", Tipo = "Receita" };
        _validatorMock.Setup(v => v.ValidateAsync(request, default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _categoriaRepositoryMock.Setup(r => r.GetAllCategorias(null))
            .ReturnsAsync(new List<Categoria>());
        _categoriaRepositoryMock.Setup(r => r.CreateCategoria(It.IsAny<Categoria>()))
            .ReturnsAsync(Result.Failure("Erro no banco de dados."));

        var result = await _categoriaService.CreateCategoriaAsync(request);

        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe("Erro no banco de dados.");
        _categoriaRepositoryMock.Verify(r => r.GetAllCategorias(null), Times.Once());
        _categoriaRepositoryMock.Verify(r => r.CreateCategoria(It.IsAny<Categoria>()), Times.Once());
        _validatorMock.Verify(v => v.ValidateAsync(request, default), Times.Once());
    }
}