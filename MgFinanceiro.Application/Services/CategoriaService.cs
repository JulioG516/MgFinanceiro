using FluentValidation;
using MgFinanceiro.Application.DTOs.Categoria;
using MgFinanceiro.Application.Interfaces;
using MgFinanceiro.Application.Mappers;
using MgFinanceiro.Domain.Common;
using MgFinanceiro.Domain.Entities;
using MgFinanceiro.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace MgFinanceiro.Application.Services;

public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IValidator<CreateCategoriaRequest> _validator;
    private readonly ILogger<CategoriaService> _logger;

    public CategoriaService(
        ICategoriaRepository categoriaRepository,
        IValidator<CreateCategoriaRequest> validator,
        ILogger<CategoriaService> logger)
    {
        _categoriaRepository = categoriaRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<CategoriaResponseDto?> GetCategoriaByIdAsync(int id)
    {
        _logger.LogInformation("Consultando categoria por ID: {CategoriaId}", id);

        try
        {
            var categoria = await _categoriaRepository.GetCategoriaByIdAsync(id);

            if (categoria == null)
            {
                _logger.LogWarning("Categoria não encontrada. ID: {CategoriaId}", id);
                return null;
            }

            _logger.LogInformation("Categoria encontrada com sucesso. ID: {CategoriaId}, Nome: {Nome}, Tipo: {Tipo}",
                categoria.Id, categoria.Nome, categoria.Tipo);

            return CategoriaMapper.CategoriaToCategoriaResponse(categoria);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao consultar categoria por ID: {CategoriaId}", id);
            throw;
        }
    }

    public async Task<PagedResponse<CategoriaResponseDto>> GetAllCategoriasAsync(int pageNumber,
        int pageSize,
        TipoCategoria? tipoCategoria = null,
        bool? statusCategoriaAtivo = null)
    {
        pageNumber = pageNumber < 1 ? 1 : pageNumber;
        pageSize = pageSize < 1 ? 10 : pageSize;
        pageSize = pageSize > 100 ? 100 : pageSize;

        _logger.LogInformation(
            "Listando todas as categorias. Filtros - Tipo: {TipoCategoria}, Ativo: {StatusAtivo}, Página: {PageNumber}, Tamanho da Página: {PageSize}",
            tipoCategoria?.ToString() ?? "Todos", statusCategoriaAtivo?.ToString() ?? "Todos", pageNumber, pageSize);

        var stopwatch = Stopwatch.StartNew();

        try
        {
            var (categorias, totalItems) =
                await _categoriaRepository.GetAllCategorias(pageNumber, pageSize, tipoCategoria, statusCategoriaAtivo);
            var categoriasList = categorias.ToList();
            var responseList = CategoriaMapper.CategoriasToCategoriaResponses(categoriasList).ToList();

            stopwatch.Stop();

            _logger.LogInformation("Consulta de categorias realizada com sucesso. " +
                                   "Total encontrado: {TotalCategorias}, Total de itens: {TotalItems}, Página: {PageNumber}, " +
                                   "Tamanho da Página: {PageSize}, Tempo de execução: {TempoMs}ms",
                responseList.Count, totalItems, pageNumber, pageSize, stopwatch.ElapsedMilliseconds);

            // Return paged response
            return new PagedResponse<CategoriaResponseDto>(responseList, pageNumber, pageSize, totalItems);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "Erro ao listar categorias. Filtros - Tipo: {TipoCategoria}, Ativo: {StatusAtivo}, " +
                                 "Página: {PageNumber}, Tamanho da Página: {PageSize}, Tempo decorrido: {TempoMs}ms",
                tipoCategoria?.ToString() ?? "Todos", statusCategoriaAtivo?.ToString() ?? "Todos",
                pageNumber, pageSize, stopwatch.ElapsedMilliseconds);
            throw;
        }
    }

    public async Task<Result<CategoriaResponseDto>> CreateCategoriaAsync(CreateCategoriaRequest request)
    {
        _logger.LogInformation("Iniciando criação de categoria. Nome: {Nome}, Tipo: {Tipo}",
            request.Nome, request.Tipo);

        // Validação
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            _logger.LogWarning("Validação falhou ao criar categoria. Erros: {ErrosValidacao}. " +
                               "Dados da requisição: {@RequestData}",
                errors, new { request.Nome, request.Tipo });

            return Result<CategoriaResponseDto>.Failure(errors);
        }

        try
        {
            var categoria = CategoriaMapper.CreateCategoriaRequestToCategoria(request);

            // Verifica duplicação
            _logger.LogDebug("Verificando se categoria já existe. Nome: {Nome}, Tipo: {Tipo}",
                categoria.Nome, categoria.Tipo);

            var exists = await _categoriaRepository.ExistsCategoriaAsync(categoria.Nome, categoria.Tipo);

            if (exists)
            {
                _logger.LogWarning("Tentativa de criar categoria duplicada. Nome: {Nome}, Tipo: {Tipo}",
                    categoria.Nome, categoria.Tipo);
                return Result<CategoriaResponseDto>.Failure("Já existe uma categoria com este nome e tipo.");
            }

            // Definir propriedades padrão
            categoria.DataCriacao = DateTime.UtcNow;
            categoria.Ativo = true;

            _logger.LogDebug("Persistindo nova categoria no repositório. Nome: {Nome}, Tipo: {Tipo}",
                categoria.Nome, categoria.Tipo);

            var result = await _categoriaRepository.CreateCategoria(categoria);

            if (result.IsSuccess)
            {
                _logger.LogInformation("Categoria criada com sucesso. ID: {CategoriaId}, Nome: {Nome}, Tipo: {Tipo}",
                    result.Value!.Id, result.Value.Nome, result.Value.Tipo);

                return Result<CategoriaResponseDto>.Success(
                    CategoriaMapper.CategoriaToCategoriaResponse(result.Value));
            }

            _logger.LogError("Falha ao criar categoria no repositório. Erro: {Erro}. " +
                             "Dados: Nome={Nome}, Tipo={Tipo}",
                result.Error, categoria.Nome, categoria.Tipo);

            return Result<CategoriaResponseDto>.Failure(result.Error);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao criar categoria. Nome: {Nome}, Tipo: {Tipo}",
                request.Nome, request.Tipo);
            return Result<CategoriaResponseDto>.Failure("Erro interno ao criar categoria.");
        }
    }

    public async Task<Result<CategoriaResponseDto>> UpdateCategoriaStatusAsync(int id,
        UpdateCategoriaStatusRequest request)
    {
        _logger.LogInformation(
            "Iniciando atualização de status da categoria. ID: {CategoriaId}, NovoStatus: {NovoStatus}",
            id, request.Ativo);

        // Validação de ID
        if (id != request.Id)
        {
            _logger.LogWarning("ID da URL não corresponde ao ID do corpo da requisição. " +
                               "URL_ID: {UrlId}, Body_ID: {BodyId}", id, request.Id);

            return Result<CategoriaResponseDto>.Failure(
                "O identificador fornecido na URL não corresponde ao especificado no corpo da requisição.");
        }

        try
        {
            // Buscar categoria existente
            _logger.LogDebug("Buscando categoria para atualização. ID: {CategoriaId}", id);

            var categoria = await _categoriaRepository.GetCategoriaByIdAsync(id);
            if (categoria == null)
            {
                _logger.LogWarning("Categoria não encontrada para atualização de status. ID: {CategoriaId}", id);
                return Result<CategoriaResponseDto>.Failure("Categoria não encontrada.");
            }

            var statusAnterior = categoria.Ativo;
            categoria.Ativo = request.Ativo;

            _logger.LogInformation("Atualizando status da categoria. ID: {CategoriaId}, " +
                                   "Status anterior: {StatusAnterior}, Novo status: {NovoStatus}",
                categoria.Id, statusAnterior, request.Ativo);

            var result = await _categoriaRepository.UpdateCategoria(categoria);

            if (result.IsSuccess)
            {
                _logger.LogInformation("Status da categoria atualizado com sucesso. " +
                                       "ID: {CategoriaId}, Nome: {Nome}, Status: {StatusAtual}",
                    result.Value!.Id, result.Value.Nome, result.Value.Ativo);

                return Result<CategoriaResponseDto>.Success(
                    CategoriaMapper.CategoriaToCategoriaResponse(result.Value));
            }

            _logger.LogError("Falha ao atualizar status da categoria no repositório. " +
                             "ID: {CategoriaId}, Erro: {Erro}", id, result.Error);

            return Result<CategoriaResponseDto>.Failure(result.Error);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao atualizar status da categoria. " +
                                 "ID: {CategoriaId}, NovoStatus: {NovoStatus}", id, request.Ativo);
            return Result<CategoriaResponseDto>.Failure("Erro interno ao atualizar categoria.");
        }
    }
}