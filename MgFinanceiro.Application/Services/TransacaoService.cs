using FluentValidation;
using MgFinanceiro.Application.DTOs.Transacao;
using MgFinanceiro.Application.Interfaces;
using MgFinanceiro.Application.Mappers;
using MgFinanceiro.Domain.Common;
using MgFinanceiro.Domain.Entities;
using MgFinanceiro.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace MgFinanceiro.Application.Services;

public class TransacaoService : ITransacaoService
{
    private readonly ITransacaoRepository _transacaoRepository;
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IValidator<CreateTransacaoRequest> _createValidator;
    private readonly IValidator<UpdateTransacaoRequest> _updateValidator;
    private readonly ILogger<TransacaoService> _logger;

    public TransacaoService(
        ITransacaoRepository transacaoRepository, 
        ICategoriaRepository categoriaRepository,
        IValidator<CreateTransacaoRequest> createValidator, 
        IValidator<UpdateTransacaoRequest> updateValidator,
        ILogger<TransacaoService> logger)
    {
        _transacaoRepository = transacaoRepository;
        _categoriaRepository = categoriaRepository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _logger = logger;
    }

public async Task<PagedResponse<TransacaoResponseDto>> GetAllTransacoesAsync(
    DateTime? dataInicio, 
    DateTime? dataFim,
    int? categoriaId, 
    TipoCategoria? tipoCategoria, 
    int pageNumber = 1, 
    int pageSize = 10)
{
    var filtros = FormatarFiltrosLog(dataInicio, dataFim, categoriaId, tipoCategoria);
    _logger.LogInformation("Consultando todas as transações. Filtros: {Filtros}, Página: {PageNumber}, Tamanho da Página: {PageSize}", 
        filtros, pageNumber, pageSize);

    var stopwatch = Stopwatch.StartNew();

    try
    {
        pageNumber = pageNumber < 1 ? 1 : pageNumber;
        pageSize = pageSize < 1 ? 10 : pageSize;
        pageSize = pageSize > 100 ? 100 : pageSize;

        var (transacoes, totalItems) = await _transacaoRepository.GetAllTransacoesAsync(
            dataInicio, dataFim, categoriaId, tipoCategoria, pageNumber, pageSize);

        var transacoesList = transacoes.ToList();
        var response = TransacaoMapper.TransacoesToTransacaoResponses(transacoesList).ToList();

        stopwatch.Stop();

        _logger.LogInformation("Consulta de transações realizada com sucesso. " +
            "Total encontrado: {TotalTransacoes}, Total de itens: {TotalItems}, Página: {PageNumber}, " +
            "Tamanho da Página: {PageSize}, Tempo de execução: {TempoMs}ms, Filtros: {Filtros}",
            response.Count, totalItems, pageNumber, pageSize, stopwatch.ElapsedMilliseconds, filtros);

        // Log de métricas financeiras
        if (transacoesList.Any())
        {
            var totalReceitas = transacoesList.Where(t => t.Categoria?.Tipo == TipoCategoria.Receita).Sum(t => t.Valor);
            var totalDespesas = transacoesList.Where(t => t.Categoria?.Tipo == TipoCategoria.Despesa).Sum(t => t.Valor);
            var saldo = totalReceitas - totalDespesas;

            _logger.LogInformation("Métricas das transações consultadas. " +
                "Total Receitas: {TotalReceitas:C}, Total Despesas: {TotalDespesas:C}, " +
                "Saldo: {Saldo:C}, Período: {Periodo}",
                totalReceitas, totalDespesas, saldo, 
                FormatarPeriodoLog(dataInicio, dataFim));
        }

        return new PagedResponse<TransacaoResponseDto>(response, pageNumber, pageSize, totalItems);
    }
    catch (Exception ex)
    {
        stopwatch.Stop();
        _logger.LogError(ex, "Erro ao consultar transações. " +
            "Filtros: {Filtros}, Página: {PageNumber}, Tamanho da Página: {PageSize}, Tempo decorrido: {TempoMs}ms", 
            filtros, pageNumber, pageSize, stopwatch.ElapsedMilliseconds);
        throw;
    }
}

    public async Task<TransacaoResponseDto?> GetTransacaoByIdAsync(int id)
    {
        _logger.LogInformation("Consultando transação por ID: {TransacaoId}", id);

        try
        {
            var transacao = await _transacaoRepository.GetTransacaoByIdAsync(id);
            
            if (transacao == null)
            {
                _logger.LogWarning("Transação não encontrada. ID: {TransacaoId}", id);
                return null;
            }

            _logger.LogInformation("Transação encontrada com sucesso. " +
                "ID: {TransacaoId}, Valor: {Valor:C}, Categoria: {CategoriaNome}, Tipo: {TipoCategoria}",
                transacao.Id, transacao.Valor, transacao.Categoria?.Nome ?? "N/A", 
                transacao.Categoria?.Tipo.ToString() ?? "N/A");

            return TransacaoMapper.MapTransacaoToResponse(transacao);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao consultar transação por ID: {TransacaoId}", id);
            throw;
        }
    }

    public async Task<Result<TransacaoResponseDto>> CreateTransacaoAsync(CreateTransacaoRequest request)
    {
        _logger.LogInformation("Iniciando criação de transação. " +
            "Valor: {Valor:C}, Descrição: {Descricao}, CategoriaId: {CategoriaId}, Data: {Data:yyyy-MM-dd}",
            request.Valor, request.Descricao, request.CategoriaId, request.Data);

        // Validação da requisição
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            _logger.LogWarning("Validação falhou ao criar transação. " +
                "Erros: {ErrosValidacao}, Dados: {@RequestData}",
                errors, new { request.Valor, request.Descricao, request.CategoriaId, request.Data });
            return Result<TransacaoResponseDto>.Failure(errors);
        }

        try
        {
            // Validação da categoria
            _logger.LogDebug("Validando categoria para transação. CategoriaId: {CategoriaId}", request.CategoriaId);
            
            var categoria = await _categoriaRepository.GetCategoriaByIdAsync(request.CategoriaId);
            if (categoria == null || !categoria.Ativo)
            {
                _logger.LogWarning("Categoria inválida ou inativa para transação. " +
                    "CategoriaId: {CategoriaId}, Existe: {CategoriaExiste}, Ativa: {CategoriaAtiva}",
                    request.CategoriaId, categoria != null, categoria?.Ativo ?? false);
                return Result<TransacaoResponseDto>.Failure("A categoria especificada não existe ou não está ativa.");
            }

            var transacao = TransacaoMapper.CreateTransacaoRequestToTransacao(request);
            transacao.Data = request.Data.ToUniversalTime();
            transacao.DataCriacao = DateTime.UtcNow;

            _logger.LogDebug("Persistindo transação no repositório. " +
                "Valor: {Valor:C}, Categoria: {CategoriaNome} ({CategoriaId})",
                transacao.Valor, categoria.Nome, categoria.Id);

            var createdResult = await _transacaoRepository.CreateTransacaoAsync(transacao);
            if (!createdResult.IsSuccess)
            {
                _logger.LogError("Falha ao criar transação no repositório. " +
                    "Erro: {Erro}, Valor: {Valor:C}, CategoriaId: {CategoriaId}",
                    createdResult.Error, request.Valor, request.CategoriaId);
                return Result<TransacaoResponseDto>.Failure(createdResult.Error);
            }

            // Para aparecer corretamente a categoria
            createdResult.Value!.Categoria = categoria;
            var dto = TransacaoMapper.MapTransacaoToResponse(createdResult.Value!);

            _logger.LogInformation("Transação criada com sucesso. " +
                "ID: {TransacaoId}, Valor: {Valor:C}, Categoria: {CategoriaNome}, " +
                "Tipo: {TipoCategoria}, Data: {DataTransacao:yyyy-MM-dd}",
                dto.Id, dto.Valor, dto.CategoriaNome, dto.CategoriaTipo, dto.Data);
            
            return Result<TransacaoResponseDto>.Success(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao criar transação. " +
                "Valor: {Valor:C}, Descrição: {Descricao}, CategoriaId: {CategoriaId}",
                request.Valor, request.Descricao, request.CategoriaId);
            return Result<TransacaoResponseDto>.Failure("Erro interno ao criar transação.");
        }
    }

    public async Task<Result> UpdateTransacaoAsync(UpdateTransacaoRequest request)
    {
        _logger.LogInformation("Iniciando atualização de transação. " +
            "ID: {TransacaoId}, Novo valor: {NovoValor:C}, Nova descrição: {NovaDescricao}",
            request.Id, request.Valor, request.Descricao);

        // Validação da requisição
        var validationResult = await _updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            _logger.LogWarning("Validação falhou ao atualizar transação. " +
                "ID: {TransacaoId}, Erros: {ErrosValidacao}",
                request.Id, errors);
            return Result.Failure(errors);
        }

        try
        {
            // Verificar se transação existe
            _logger.LogDebug("Verificando existência da transação. ID: {TransacaoId}", request.Id);
            
            var transacaoExistente = await _transacaoRepository.GetTransacaoByIdAsync(request.Id);
            if (transacaoExistente == null)
            {
                _logger.LogWarning("Transação não encontrada para atualização. ID: {TransacaoId}", request.Id);
                return Result.Failure("Transação não encontrada.");
            }

            // Log dos valores anteriores para auditoria
            _logger.LogDebug("Valores anteriores da transação. " +
                "ID: {TransacaoId}, Valor anterior: {ValorAnterior:C}, " +
                "Descrição anterior: {DescricaoAnterior}, Categoria anterior: {CategoriaAnterior}",
                transacaoExistente.Id, transacaoExistente.Valor, 
                transacaoExistente.Descricao, transacaoExistente.Categoria?.Nome ?? "N/A");

            // Validação da nova categoria
            _logger.LogDebug("Validando nova categoria. CategoriaId: {CategoriaId}", request.CategoriaId);
            
            var categoria = await _categoriaRepository.GetCategoriaByIdAsync(request.CategoriaId);
            if (categoria == null || !categoria.Ativo)
            {
                _logger.LogWarning("Categoria inválida ou inativa para atualização. " +
                    "TransacaoId: {TransacaoId}, CategoriaId: {CategoriaId}, " +
                    "Existe: {CategoriaExiste}, Ativa: {CategoriaAtiva}",
                    request.Id, request.CategoriaId, categoria != null, categoria?.Ativo ?? false);
                return Result.Failure("A categoria especificada não existe ou não está ativa.");
            }

            var transacao = TransacaoMapper.UpdateTransacaoRequestToTransacao(request);
            transacao.Data = request.Data.ToUniversalTime();
            transacao.DataCriacao = transacaoExistente.DataCriacao.ToUniversalTime();

            var updateResult = await _transacaoRepository.UpdateTransacaoAsync(transacao);
            
            if (updateResult.IsSuccess)
            {
                _logger.LogInformation("Transação atualizada com sucesso. " +
                    "ID: {TransacaoId}, Valor: {ValorAtual:C}, Categoria: {CategoriaNome}",
                    request.Id, request.Valor, categoria.Nome);
            }
            else
            {
                _logger.LogError("Falha ao atualizar transação no repositório. " +
                    "ID: {TransacaoId}, Erro: {Erro}", request.Id, updateResult.Error);
            }

            return updateResult;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao atualizar transação. " +
                "ID: {TransacaoId}, Valor: {Valor:C}", request.Id, request.Valor);
            return Result.Failure("Erro interno ao atualizar transação.");
        }
    }

    public async Task<Result> DeleteTransacaoAsync(int id)
    {
        _logger.LogInformation("Iniciando exclusão de transação. ID: {TransacaoId}", id);

        try
        {
            // Buscar transação para log de auditoria antes da exclusão
            var transacao = await _transacaoRepository.GetTransacaoByIdAsync(id);
            if (transacao != null)
            {
                _logger.LogInformation("Transação encontrada para exclusão. " +
                    "ID: {TransacaoId}, Valor: {Valor:C}, Descrição: {Descricao}, " +
                    "Categoria: {CategoriaNome}",
                    transacao.Id, transacao.Valor, transacao.Descricao, 
                    transacao.Categoria?.Nome ?? "N/A");
            }
            else
            {
                _logger.LogWarning("Transação não encontrada para exclusão. ID: {TransacaoId}", id);
            }

            var result = await _transacaoRepository.DeleteTransacaoAsync(id);

            if (result.IsSuccess)
            {
                _logger.LogInformation("Transação excluída com sucesso. ID: {TransacaoId}", id);
            }
            else
            {
                _logger.LogError("Falha ao excluir transação. ID: {TransacaoId}, Erro: {Erro}", 
                    id, result.Error);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao excluir transação. ID: {TransacaoId}", id);
            return Result.Failure("Erro interno ao excluir transação.");
        }
    }

    private static string FormatarFiltrosLog(DateTime? dataInicio, DateTime? dataFim, int? categoriaId, TipoCategoria? tipoCategoria)
    {
        var filtros = new List<string>();
        
        if (dataInicio.HasValue)
            filtros.Add($"DataInício={dataInicio.Value:yyyy-MM-dd}");
        
        if (dataFim.HasValue)
            filtros.Add($"DataFim={dataFim.Value:yyyy-MM-dd}");
        
        if (categoriaId.HasValue)
            filtros.Add($"CategoriaId={categoriaId.Value}");
        
        if (tipoCategoria.HasValue)
            filtros.Add($"TipoCategoria={tipoCategoria.Value}");

        return filtros.Any() ? string.Join(", ", filtros) : "Nenhum filtro";
    }

    private static string FormatarPeriodoLog(DateTime? dataInicio, DateTime? dataFim)
    {
        if (dataInicio.HasValue && dataFim.HasValue)
            return $"{dataInicio.Value:yyyy-MM-dd} a {dataFim.Value:yyyy-MM-dd}";
        
        if (dataInicio.HasValue)
            return $"A partir de {dataInicio.Value:yyyy-MM-dd}";
        
        if (dataFim.HasValue)
            return $"Até {dataFim.Value:yyyy-MM-dd}";
        
        return "Sem período definido";
    }
}