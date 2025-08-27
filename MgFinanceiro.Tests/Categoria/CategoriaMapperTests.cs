using MgFinanceiro.Application.DTOs.Categoria;
using MgFinanceiro.Application.Mappers;
using MgFinanceiro.Domain.Entities;
using Shouldly;

namespace MgFinanceiro.Tests.Categoria;

public class CategoriaMapperTests
{
    # region Create Request to Categoria

    [Fact]
    public void Should_Have_Error_Mapping_To_Categoria_With_Invalid_DTO_Tipo()
    {
        var request = new CreateCategoriaRequest
        {
            Nome = "Nova Categoria",
            Tipo = "Errado"
        };

        var exception = Should.Throw<ArgumentException>(() =>
        {
            _ = CategoriaMapper.CreateCategoriaRequestToCategoria(request);
        });
        exception.Message.ShouldBe("TipoCategoria inválido: Errado");
    }

    [Fact]
    public void Should_Map_To_Categoria_With_Valid_DTO()
    {
        var request = new CreateCategoriaRequest
        {
            Nome = "Nova Categoria",
            Tipo = "Receita"
        };

        var categoria = CategoriaMapper.CreateCategoriaRequestToCategoria(request);
        categoria.Nome.ShouldBe("Nova Categoria");
        categoria.Tipo.ShouldBe(TipoCategoria.Receita);
    }

    #endregion

    #region Categoria to Categoria Response DTO

    [Fact]
    public void Should_Map_Categoria_To_CategoriaResponse()
    {
        var createdAt = DateTime.UtcNow;

        var categoria = new Domain.Entities.Categoria
        {
            Id = 1,
            Nome = "Categoria de Transacao",
            Tipo = TipoCategoria.Despesa,
            Ativo = true,
            DataCriacao = createdAt,
            Transacoes = new List<Transacao>()
        };

        // ignora transacoes
        var dto = CategoriaMapper.CategoriaToCategoriaResponse(categoria);
        dto.Id.ShouldBe(1);
        dto.Nome.ShouldBe("Categoria de Transacao");
        dto.Tipo = "Despesa";
        dto.Ativo = true;
        dto.DataCriacao = createdAt;
    }

    [Fact]
    public void Should_Map_Categoria_List_To_CategoriaResponse_List()
    {
        var createdAt1 = DateTime.UtcNow;
        var createdAt2 = DateTime.UtcNow.AddDays(-5);

        var categorias = new List<Domain.Entities.Categoria>()
        {
            new()
            {
                Id = 1,
                Nome = "Categoria 1",
                Tipo = TipoCategoria.Despesa,
                Ativo = true,
                DataCriacao = createdAt1,
                Transacoes = new List<Transacao>()
            },
            new()
            {
                Id = 2,
                Nome = "Categoria 2",
                Tipo = TipoCategoria.Receita,
                Ativo = true,
                DataCriacao = createdAt2,
                Transacoes = new List<Transacao>()
            }
        };

        var responses = (CategoriaMapper.CategoriasToCategoriaResponses(categorias)).ToList();
        responses.ShouldNotBeNull();
        responses.ShouldNotBeEmpty();
        responses.Count.ShouldBe(2);
        responses.ShouldContain(x => x.Id == 1 && x.Nome == "Categoria 1" && x.Tipo == "Despesa");
        responses.ShouldContain(x => x.Id == 2 && x.Nome == "Categoria 2" && x.Tipo == "Receita");
    }

    #endregion
}