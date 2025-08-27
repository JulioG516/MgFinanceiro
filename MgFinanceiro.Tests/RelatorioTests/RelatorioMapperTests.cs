using MgFinanceiro.Application.Mappers;
using MgFinanceiro.Domain.Entities;
using Shouldly;

namespace MgFinanceiro.Tests.RelatorioTests;

public class RelatorioMapperTests
{
    #region Relatorio Resumo 

    

    
    [Fact]
    public void Should_Map_RelatorioResumo_To_RelatorioResumoResponse()
    {
        var resumo = new RelatorioResumo
        {
            Ano = 2025,
            Mes = 8,
            SaldoTotal = 500.50m,
            TotalReceitas = 1000.75m,
            TotalDespesas = 500.25m
        };

        var responses = RelatorioMapper.MapResumos(new[] { resumo }).ToList();

        responses.ShouldNotBeNull();
        responses.ShouldNotBeEmpty();
        responses.Count.ShouldBe(1);
        var response = responses[0];
        response.Ano.ShouldBe(2025);
        response.Mes.ShouldBe(8);
        response.SaldoTotal.ShouldBe(500.50m);
        response.TotalReceitas.ShouldBe(1000.75m);
        response.TotalDespesas.ShouldBe(500.25m);
    }
    
    [Fact]
    public void Should_Map_RelatorioResumo_List_To_RelatorioResumoResponse_List()
    {
        var resumos = new List<RelatorioResumo>
        {
            new()
            {
                Ano = 2025,
                Mes = 8,
                SaldoTotal = 500.50m,
                TotalReceitas = 1000.75m,
                TotalDespesas = 500.25m
            },
            new()
            {
                Ano = 2025,
                Mes = 9,
                SaldoTotal = -200.00m,
                TotalReceitas = 300.00m,
                TotalDespesas = 500.00m
            }
        };

        var responses = RelatorioMapper.MapResumos(resumos).ToList();

        responses.ShouldNotBeNull();
        responses.ShouldNotBeEmpty();
        responses.Count.ShouldBe(2);
        responses.ShouldContain(r => r.Ano == 2025 && r.Mes == 8 && r.SaldoTotal == 500.50m);
        responses.ShouldContain(r => r.Ano == 2025 && r.Mes == 9 && r.SaldoTotal == -200.00m);
    }
    
    [Fact]
    public void Should_Map_Empty_RelatorioResumo_List_To_Empty_RelatorioResumoResponse_List()
    {
        var resumos = new List<RelatorioResumo>();

        var responses = RelatorioMapper.MapResumos(resumos).ToList();

        responses.ShouldNotBeNull();
        responses.ShouldBeEmpty();
    }
    #endregion
    
    # region Relatorio Por Categoria
    [Fact]
    public void Should_Map_RelatorioPorCategoria_To_RelatorioPorCategoriaResponse()
    {
        var relatorio = new RelatorioPorCategoria
        {
            Ano = 2025,
            Mes = 8,
            CategoriaId = 1,
            CategoriaNome = "Vendas",
            CategoriaTipo = TipoCategoria.Receita,
            Total = 1000.75m
        };

        var responses = RelatorioMapper.MapPorCategorias(new[] { relatorio }).ToList();

        responses.ShouldNotBeNull();
        responses.ShouldNotBeEmpty();
        responses.Count.ShouldBe(1);
        var response = responses[0];
        response.Ano.ShouldBe(2025);
        response.Mes.ShouldBe(8);
        response.CategoriaId.ShouldBe(1);
        response.CategoriaNome.ShouldBe("Vendas");
        response.CategoriaTipo.ShouldBe("Receita");
        response.Total.ShouldBe(1000.75m);
    }
    [Fact]
    public void Should_Map_RelatorioPorCategoria_List_To_RelatorioPorCategoriaResponse_List()
    {
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
            },
            new()
            {
                Ano = 2025,
                Mes = 8,
                CategoriaId = 2,
                CategoriaNome = "Despesas Operacionais",
                CategoriaTipo = TipoCategoria.Despesa,
                Total = 500.25m
            }
        };

        var responses = RelatorioMapper.MapPorCategorias(relatorios).ToList();

        responses.ShouldNotBeNull();
        responses.ShouldNotBeEmpty();
        responses.Count.ShouldBe(2);
        responses.ShouldContain(r => r.CategoriaId == 1 && r.CategoriaNome == "Vendas" && r.CategoriaTipo == "Receita");
        responses.ShouldContain(r => r.CategoriaId == 2 && r.CategoriaNome == "Despesas Operacionais" && r.CategoriaTipo == "Despesa");
    }

    [Fact]
    public void Should_Map_Empty_RelatorioPorCategoria_List_To_Empty_RelatorioPorCategoriaResponse_List()
    {
        var relatorios = new List<RelatorioPorCategoria>();

        var responses = RelatorioMapper.MapPorCategorias(relatorios).ToList();

        responses.ShouldNotBeNull();
        responses.ShouldBeEmpty();
    }
    #endregion
}