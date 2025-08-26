using FluentValidation;
using MgFinanceiro.Application.DTOs.Relatorio;
using MgFinanceiro.Application.Interfaces;
using MgFinanceiro.Application.Mappers;
using MgFinanceiro.Domain.Common;
using MgFinanceiro.Domain.Entities;
using MgFinanceiro.Domain.Interfaces;

namespace MgFinanceiro.Application.Services;

public class RelatorioService : IRelatorioService
{
    private readonly IRelatorioRepository _relatorioRepository;
    private readonly IValidator<RelatorioQueryDto> _queryValidator;

    public RelatorioService(IRelatorioRepository relatorioRepository, IValidator<RelatorioQueryDto> queryValidator)
    {
        _relatorioRepository = relatorioRepository;
        _queryValidator = queryValidator;
    }

    public async Task<Result<IEnumerable<RelatorioResumoResponse>>> GetRelatorioResumoAsync(RelatorioQueryDto query)
    {
        var validationResult = await _queryValidator.ValidateAsync(query);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return Result<IEnumerable<RelatorioResumoResponse>>.Failure(errors);
        }

        IEnumerable<RelatorioResumo> resumo;
        if (!query.Ano.HasValue && !query.Mes.HasValue)
        {
            resumo = await _relatorioRepository.GetResumoAnoAtualAsync();
        }
        else
        {
            resumo = await _relatorioRepository.GetResumoPorPeriodoAsync(query.Ano!.Value, query.Mes);
        }

        return Result<IEnumerable<RelatorioResumoResponse>>.Success(RelatorioMapper.MapResumos(resumo));
    }

    public async Task<Result<IEnumerable<RelatorioPorCategoriaResponse>>> GetRelatorioPorCategoriaAsync(RelatorioQueryDto query)
    {
        var validationResult = await _queryValidator.ValidateAsync(query);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return Result<IEnumerable<RelatorioPorCategoriaResponse>>.Failure(errors);
        }

        IEnumerable<RelatorioPorCategoria> porCategoria;
        if (!query.Ano.HasValue && !query.Mes.HasValue)
        {
            porCategoria = await _relatorioRepository.GetPorCategoriaAnoAtualAsync();
        }
        else
        {
            porCategoria = await _relatorioRepository.GetPorCategoriaPorPeriodoAsync(query.Ano!.Value, query.Mes);
        }

        return Result<IEnumerable<RelatorioPorCategoriaResponse>>.Success(
            RelatorioMapper.MapPorCategorias(porCategoria));
    }
}