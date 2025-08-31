using FluentValidation;
using MgFinanceiro.Application.DTOs.Relatorio;
using MgFinanceiro.Application.Interfaces;
using MgFinanceiro.Application.Mappers;
using MgFinanceiro.Domain.Common;
using MgFinanceiro.Domain.Entities;
using MgFinanceiro.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace MgFinanceiro.Application.Services;

public class RelatorioService : IRelatorioService
{
    private readonly IRelatorioRepository _relatorioRepository;
    private readonly IValidator<RelatorioQueryDto> _queryValidator;
    private readonly ILogger<RelatorioService> _logger;

    public RelatorioService(
        IRelatorioRepository relatorioRepository, 
        IValidator<RelatorioQueryDto> queryValidator,
        ILogger<RelatorioService> logger)
    {
        _relatorioRepository = relatorioRepository;
        _queryValidator = queryValidator;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<RelatorioResumoResponse>>> GetRelatorioResumoAsync(RelatorioQueryDto query)
    {
        var periodo = FormatarPeriodoLog(query.Ano, query.Mes);
        _logger.LogInformation("Iniciando geração de relatório resumo. Período: {Periodo}", periodo);

        // Validação
        var validationResult = await _queryValidator.ValidateAsync(query);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            _logger.LogWarning("Validação falhou para relatório resumo. Período: {Periodo}, Erros: {ErrosValidacao}", 
                periodo, errors);
            return Result<IEnumerable<RelatorioResumoResponse>>.Failure(errors);
        }

        var stopwatch = Stopwatch.StartNew();

        try
        {
            IEnumerable<RelatorioResumo> resumo;
            
            if (!query.Ano.HasValue && !query.Mes.HasValue)
            {
                _logger.LogDebug("Buscando relatório resumo para ano atual");
                resumo = await _relatorioRepository.GetResumoAnoAtualAsync();
            }
            else
            {
                _logger.LogDebug("Buscando relatório resumo para período específico. Ano: {Ano}, Mês: {Mes}", 
                    query.Ano, query.Mes);
                resumo = await _relatorioRepository.GetResumoPorPeriodoAsync(query.Ano!.Value, query.Mes);
            }

            var resumoList = resumo.ToList();
            var response = RelatorioMapper.MapResumos(resumoList).ToList();
            stopwatch.Stop();

            _logger.LogInformation("Relatório resumo gerado com sucesso. " +
                "Período: {Periodo}, Registros encontrados: {TotalRegistros}, " +
                "Tempo de execução: {TempoMs}ms",
                periodo, response.Count, stopwatch.ElapsedMilliseconds);

            // Log de métricas do relatório
            if (resumoList.Any())
            {
                var totalReceitas = resumoList.Sum(r => r.TotalReceitas);
                var totalDespesas = resumoList.Sum(r => r.TotalDespesas);
                var saldo = totalReceitas - totalDespesas;

                _logger.LogInformation("Métricas do relatório resumo. " +
                    "Total Receitas: {TotalReceitas:C}, Total Despesas: {TotalDespesas:C}, " +
                    "Saldo: {Saldo:C}, Período: {Periodo}",
                    totalReceitas, totalDespesas, saldo, periodo);
            }
            else
            {
                _logger.LogInformation("Nenhum dado encontrado para o relatório resumo. Período: {Periodo}", periodo);
            }

            return Result<IEnumerable<RelatorioResumoResponse>>.Success(response);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "Erro ao gerar relatório resumo. " +
                "Período: {Periodo}, Tempo decorrido: {TempoMs}ms", 
                periodo, stopwatch.ElapsedMilliseconds);
            return Result<IEnumerable<RelatorioResumoResponse>>.Failure("Erro interno ao gerar relatório resumo.");
        }
    }

    public async Task<Result<IEnumerable<RelatorioPorCategoriaResponse>>> GetRelatorioPorCategoriaAsync(RelatorioQueryDto query)
    {
        var periodo = FormatarPeriodoLog(query.Ano, query.Mes);
        _logger.LogInformation("Iniciando geração de relatório por categoria. Período: {Periodo}", periodo);

        // Validação
        var validationResult = await _queryValidator.ValidateAsync(query);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            _logger.LogWarning("Validação falhou para relatório por categoria. " +
                "Período: {Periodo}, Erros: {ErrosValidacao}", periodo, errors);
            return Result<IEnumerable<RelatorioPorCategoriaResponse>>.Failure(errors);
        }

        var stopwatch = Stopwatch.StartNew();

        try
        {
            IEnumerable<RelatorioPorCategoria> porCategoria;
            
            if (!query.Ano.HasValue && !query.Mes.HasValue)
            {
                _logger.LogDebug("Buscando relatório por categoria para ano atual");
                porCategoria = await _relatorioRepository.GetPorCategoriaAnoAtualAsync();
            }
            else
            {
                _logger.LogDebug("Buscando relatório por categoria para período específico. " +
                    "Ano: {Ano}, Mês: {Mes}", query.Ano, query.Mes);
                porCategoria = await _relatorioRepository.GetPorCategoriaPorPeriodoAsync(query.Ano!.Value, query.Mes);
            }

            var porCategoriaList = porCategoria.ToList();
            var response = RelatorioMapper.MapPorCategorias(porCategoriaList).ToList();
            stopwatch.Stop();

            _logger.LogInformation("Relatório por categoria gerado com sucesso. " +
                "Período: {Periodo}, Categorias encontradas: {TotalCategorias}, " +
                "Tempo de execução: {TempoMs}ms",
                periodo, response.Count, stopwatch.ElapsedMilliseconds);

            // Log de métricas detalhadas por categoria
            if (porCategoriaList.Any())
            {
                var totalGeral = porCategoriaList.Sum(c => c.Total);
                var categoriaComMaiorValor = porCategoriaList.OrderByDescending(c => c.Total).FirstOrDefault();
                
                _logger.LogInformation("Métricas do relatório por categoria. " +
                    "Total geral: {TotalGeral:C}, Categoria com maior valor: {CategoriaMaiorValor} ({ValorMaiorCategoria:C}), " +
                    "Período: {Periodo}",
                    totalGeral, 
                    categoriaComMaiorValor?.CategoriaNome ?? "N/A", 
                    categoriaComMaiorValor?.Total ?? 0, 
                    periodo);

                // Log das top 3 categorias para debug
                var top3Categorias = porCategoriaList
                    .OrderByDescending(c => c.Total)
                    .Take(3)
                    .Select(c => new { c.CategoriaNome, c.Total });

                _logger.LogDebug("Top 3 categorias por valor. {@Top3Categorias}, Período: {Periodo}", 
                    top3Categorias, periodo);
            }
            else
            {
                _logger.LogInformation("Nenhum dado encontrado para o relatório por categoria. Período: {Periodo}", periodo);
            }

            return Result<IEnumerable<RelatorioPorCategoriaResponse>>.Success(response);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "Erro ao gerar relatório por categoria. " +
                "Período: {Periodo}, Tempo decorrido: {TempoMs}ms", 
                periodo, stopwatch.ElapsedMilliseconds);
            return Result<IEnumerable<RelatorioPorCategoriaResponse>>.Failure("Erro interno ao gerar relatório por categoria.");
        }
    }

    private static string FormatarPeriodoLog(int? ano, int? mes)
    {
        if (!ano.HasValue && !mes.HasValue)
            return "Ano atual";
        
        if (ano.HasValue && !mes.HasValue)
            return $"Ano {ano.Value}";
        
        if (ano.HasValue && mes.HasValue)
            return $"{mes.Value:D2}/{ano.Value}";
        
        return "Período inválido";
    }
}