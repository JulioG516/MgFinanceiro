using FluentValidation;
using MgFinanceiro.Application.DTOs.Relatorio;
using MgFinanceiro.Application.Interfaces;
using MgFinanceiro.Application.Mappers;
using MgFinanceiro.Domain.Common;
using MgFinanceiro.Domain.Entities;
using MgFinanceiro.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using ClosedXML.Excel;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

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

    public async Task<Result<IEnumerable<RelatorioPorCategoriaResponse>>> GetRelatorioPorCategoriaAsync(
        RelatorioQueryDto query)
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
                _logger.LogInformation("Nenhum dado encontrado para o relatório por categoria. Período: {Periodo}",
                    periodo);
            }

            return Result<IEnumerable<RelatorioPorCategoriaResponse>>.Success(response);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "Erro ao gerar relatório por categoria. " +
                                 "Período: {Periodo}, Tempo decorrido: {TempoMs}ms",
                periodo, stopwatch.ElapsedMilliseconds);
            return Result<IEnumerable<RelatorioPorCategoriaResponse>>.Failure(
                "Erro interno ao gerar relatório por categoria.");
        }
    }


    public async Task<Result<byte[]>> ExportRelatorioResumoAsync(RelatorioQueryDto query, string formato)
    {
        var periodo = FormatarPeriodoLog(query.Ano, query.Mes);
        _logger.LogInformation("Iniciando exportação de relatório resumo. Formato: {Formato}, Período: {Periodo}",
            formato, periodo);

        // Validação
        var validationResult = await _queryValidator.ValidateAsync(query);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            _logger.LogWarning(
                "Validação falhou para exportação de relatório resumo. Período: {Periodo}, Erros: {ErrosValidacao}",
                periodo, errors);
            return Result<byte[]>.Failure(errors);
        }

        var stopwatch = Stopwatch.StartNew();

        try
        {
            var resumoResult = await GetRelatorioResumoAsync(query);
            if (!resumoResult.IsSuccess)
            {
                _logger.LogWarning("Falha ao obter dados do relatório resumo para exportação. Período: {Periodo}",
                    periodo);
                return Result<byte[]>.Failure(resumoResult.Error);
            }

            var resumoList = resumoResult.Value!.ToList();
            byte[] fileContent;

            switch (formato.ToLower())
            {
                case "pdf":
                    fileContent = GenerateResumoPdf(resumoList, periodo);
                    break;
                case "xlsx":
                    fileContent = GenerateResumoExcel(resumoList, periodo);
                    break;
                default:
                    _logger.LogWarning("Formato de exportação inválido: {Formato}, Período: {Periodo}", formato,
                        periodo);
                    return Result<byte[]>.Failure("Formato de exportação inválido. Use 'pdf' ou 'xlsx'.");
            }

            stopwatch.Stop();
            _logger.LogInformation(
                "Relatório resumo exportado com sucesso. Formato: {Formato}, Período: {Periodo}, Tempo de execução: {TempoMs}ms",
                formato, periodo, stopwatch.ElapsedMilliseconds);

            return Result<byte[]>.Success(fileContent);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex,
                "Erro ao exportar relatório resumo. Formato: {Formato}, Período: {Periodo}, Tempo decorrido: {TempoMs}ms",
                formato, periodo, stopwatch.ElapsedMilliseconds);
            return Result<byte[]>.Failure("Erro interno ao exportar relatório resumo.");
        }
    }

    public async Task<Result<byte[]>> ExportRelatorioPorCategoriaAsync(RelatorioQueryDto query, string formato)
    {
        var periodo = FormatarPeriodoLog(query.Ano, query.Mes);
        _logger.LogInformation(
            "Iniciando exportação de relatório por categoria. Formato: {Formato}, Período: {Periodo}", formato,
            periodo);

        // Validação
        var validationResult = await _queryValidator.ValidateAsync(query);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            _logger.LogWarning(
                "Validação falhou para exportação de relatório por categoria. Período: {Periodo}, Erros: {ErrosValidacao}",
                periodo, errors);
            return Result<byte[]>.Failure(errors);
        }

        var stopwatch = Stopwatch.StartNew();

        try
        {
            var porCategoriaResult = await GetRelatorioPorCategoriaAsync(query);
            if (!porCategoriaResult.IsSuccess)
            {
                _logger.LogWarning(
                    "Falha ao obter dados do relatório por categoria para exportação. Período: {Periodo}", periodo);
                return Result<byte[]>.Failure(porCategoriaResult.Error);
            }

            var porCategoriaList = porCategoriaResult.Value!.ToList();
            byte[] fileContent;

            switch (formato.ToLower())
            {
                case "pdf":
                    fileContent = GeneratePorCategoriaPdf(porCategoriaList, periodo);
                    break;
                case "xlsx":
                    fileContent = GeneratePorCategoriaExcel(porCategoriaList, periodo);
                    break;
                default:
                    _logger.LogWarning("Formato de exportação inválido: {Formato}, Período: {Periodo}", formato,
                        periodo);
                    return Result<byte[]>.Failure("Formato de exportação inválido. Use 'pdf' ou 'xlsx'.");
            }

            stopwatch.Stop();
            _logger.LogInformation(
                "Relatório por categoria exportado com sucesso. Formato: {Formato}, Período: {Periodo}, Tempo de execução: {TempoMs}ms",
                formato, periodo, stopwatch.ElapsedMilliseconds);

            return Result<byte[]>.Success(fileContent);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex,
                "Erro ao exportar relatório por categoria. Formato: {Formato}, Período: {Periodo}, Tempo decorrido: {TempoMs}ms",
                formato, periodo, stopwatch.ElapsedMilliseconds);
            return Result<byte[]>.Failure("Erro interno ao exportar relatório por categoria.");
        }
    }

    private byte[] GenerateResumoPdf(IEnumerable<RelatorioResumoResponse> resumoList, string periodo)
    {
        using var memoryStream = new MemoryStream();
        using var writer = new PdfWriter(memoryStream);
        using var pdf = new PdfDocument(writer);
        var document = new Document(pdf);

        document.Add(new Paragraph($"Relatório Resumo - Período: {periodo}")
            .SetFontSize(16)
            .SetTextAlignment(TextAlignment.CENTER));

        var table = new Table(UnitValue.CreatePercentArray(new float[] { 20, 20, 20, 20 }))
            .UseAllAvailableWidth();
        table.AddHeaderCell("Mês/Ano");
        table.AddHeaderCell("Total Receitas");
        table.AddHeaderCell("Total Despesas");
        table.AddHeaderCell("Saldo");

        foreach (var item in resumoList)
        {
            table.AddCell($"{item.Mes}/{item.Ano}");
            table.AddCell(item.TotalReceitas.ToString("C"));
            table.AddCell(item.TotalDespesas.ToString("C"));
            table.AddCell((item.TotalReceitas - item.TotalDespesas).ToString("C"));
        }

        document.Add(table);
        document.Close();

        return memoryStream.ToArray();
    }

    private byte[] GenerateResumoExcel(IEnumerable<RelatorioResumoResponse> resumoList, string periodo)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add($"Relatório Resumo");

        // Cabeçalho
        worksheet.Cell(1, 1).Value = $"Relatório Resumo - Período: {periodo}";
        worksheet.Range(1, 1, 1, 5).Merge().Style.Font.SetBold().Font.SetFontSize(14);
        worksheet.Cell(2, 1).Value = "Mês/Ano";
        worksheet.Cell(2, 2).Value = "Total Receitas";
        worksheet.Cell(2, 3).Value = "Total Despesas";
        worksheet.Cell(2, 4).Value = "Saldo";
        worksheet.Range(2, 1, 2, 5).Style.Font.SetBold();

        // Dados
        int row = 3;
        foreach (var item in resumoList)
        {
            worksheet.Cell(row, 1).Value = $"{item.Mes}/{item.Ano}";
            worksheet.Cell(row, 2).Value = item.TotalReceitas;
            worksheet.Cell(row, 3).Value = item.TotalDespesas;
            worksheet.Cell(row, 4).FormulaA1 = $"=B{row}-C{row}";
            row++;
        }

        worksheet.Columns().AdjustToContents();

        using var memoryStream = new MemoryStream();
        workbook.SaveAs(memoryStream);
        return memoryStream.ToArray();
    }

    private byte[] GeneratePorCategoriaPdf(IEnumerable<RelatorioPorCategoriaResponse> porCategoriaList, string periodo)
    {
        using var memoryStream = new MemoryStream();
        using var writer = new PdfWriter(memoryStream);
        using var pdf = new PdfDocument(writer);
        var document = new Document(pdf);

        document.Add(new Paragraph($"Relatório por Categoria - Período: {periodo}")
            .SetFontSize(16)
            .SetTextAlignment(TextAlignment.CENTER));

        var table = new Table(UnitValue.CreatePercentArray(new float[] { 70, 30 }))
            .UseAllAvailableWidth();
        table.AddHeaderCell("Categoria");
        table.AddHeaderCell("Total");

        foreach (var item in porCategoriaList)
        {
            table.AddCell($"{item.CategoriaNome} ({item.CategoriaTipo})");
            table.AddCell(item.Total.ToString("C"));
        }

        document.Add(table);
        document.Close();

        return memoryStream.ToArray();
    }

    private byte[] GeneratePorCategoriaExcel(IEnumerable<RelatorioPorCategoriaResponse> porCategoriaList,
        string periodo)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Relatório por Categoria");

        // Cabeçalho
        worksheet.Cell(1, 1).Value = $"Relatório por Categoria - Período: {periodo}";
        worksheet.Range(1, 1, 1, 2).Merge().Style.Font.SetBold().Font.SetFontSize(14);
        worksheet.Cell(2, 1).Value = "Categoria";
        worksheet.Cell(2, 2).Value = "Total";
        worksheet.Range(2, 1, 2, 2).Style.Font.SetBold();

        // Dados
        int row = 3;
        foreach (var item in porCategoriaList)
        {
            worksheet.Cell(row, 1).Value = $"{item.CategoriaNome} ({item.CategoriaTipo})";
            worksheet.Cell(row, 2).Value = item.Total;
            row++;
        }

        worksheet.Columns().AdjustToContents();

        using var memoryStream = new MemoryStream();
        workbook.SaveAs(memoryStream);
        return memoryStream.ToArray();
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