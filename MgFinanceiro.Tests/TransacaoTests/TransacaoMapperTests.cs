using MgFinanceiro.Application.DTOs.Transacao;
using MgFinanceiro.Application.Mappers;
using MgFinanceiro.Domain.Entities;
using Shouldly;

namespace MgFinanceiro.Tests.TransacaoTests;

public class TransacaoMapperTests
{
    [Fact]
    public void Should_Map_To_Transacao_With_Valid_Create_Request()
    {
        var createdAt = DateTime.UtcNow;

        var request = new CreateTransacaoRequest
        {
            Descricao = "Descricao da Transacao",
            Valor = new decimal(250.25),
            Data = createdAt,
            CategoriaId = 1,
            Observacoes = "Observacoes da Transacao"
        };

        var transacao = TransacaoMapper.CreateTransacaoRequestToTransacao(request);
        transacao.ShouldNotBeNull();
        transacao.Descricao.ShouldBe("Descricao da Transacao");
        transacao.Valor.ShouldBe((decimal)250.25);
        transacao.Data.ShouldBe(createdAt);
        transacao.CategoriaId.ShouldBe(1);
        transacao.Observacoes.ShouldBe("Observacoes da Transacao");
    }

    [Fact]
    public void Should_Map_To_Transacao_With_Valid_Update_Request()
    {
        var createdAt = DateTime.UtcNow;

        var request = new UpdateTransacaoRequest
        {
            Id = 1,
            Descricao = "Atualizando A Transacao",
            Valor = new decimal(500.25),
            Data = createdAt,
            CategoriaId = 1,
            Observacoes = "Observacoes da Transacao Atualizada"
        };

        var transacao = TransacaoMapper.UpdateTransacaoRequestToTransacao(request);
        transacao.ShouldNotBeNull();
        transacao.Id.ShouldBe(1);
        transacao.Descricao.ShouldBe("Atualizando A Transacao");
        transacao.Valor.ShouldBe((decimal)500.25);
        transacao.Data.ShouldBe(createdAt);
        transacao.CategoriaId.ShouldBe(1);
        transacao.Observacoes.ShouldBe("Observacoes da Transacao Atualizada");
    }

    [Fact]
    public void Should_Map_Transacao_To_TransacaoResponseDto()
    {
        var createdAt = DateTime.UtcNow;

        var transacao = new Transacao
        {
            Id = 1,
            Descricao = "Descricao da Transacao",
            Valor = 500,
            Data = new DateTime(2024, 12, 1),
            CategoriaId = 1,
            Observacoes = "Observacoes da Transacao",
            DataCriacao = createdAt,
            Categoria = new Categoria
            {
                Id = 1,
                Nome = "Nome de Categoria",
                Tipo = TipoCategoria.Receita,
                Ativo = true,
                DataCriacao = DateTime.UtcNow,
                Transacoes = new List<Transacao>()
            }
        };

        var responseDto = TransacaoMapper.MapTransacaoToResponse(transacao);
        responseDto.ShouldNotBeNull();
        responseDto.Descricao.ShouldBe("Descricao da Transacao");
        responseDto.Valor.ShouldBe(500);
        responseDto.Data.ShouldBe(new DateTime(2024, 12, 1));
        responseDto.CategoriaId.ShouldBe(1);
        responseDto.Observacoes.ShouldBe("Observacoes da Transacao");
        responseDto.DataCriacao.ShouldBe(createdAt);
        responseDto.CategoriaNome.ShouldBe("Nome de Categoria");
        responseDto.CategoriaTipo.ShouldBe("Receita");
    }
}