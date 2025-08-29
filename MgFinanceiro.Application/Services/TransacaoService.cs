using FluentValidation;
using MgFinanceiro.Application.DTOs.Transacao;
using MgFinanceiro.Application.Interfaces;
using MgFinanceiro.Application.Mappers;
using MgFinanceiro.Domain.Common;
using MgFinanceiro.Domain.Entities;
using MgFinanceiro.Domain.Interfaces;

namespace MgFinanceiro.Application.Services;

public class TransacaoService : ITransacaoService
{
    private readonly ITransacaoRepository _transacaoRepository;
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IValidator<CreateTransacaoRequest> _createValidator;
    private readonly IValidator<UpdateTransacaoRequest> _updateValidator;

    public TransacaoService(ITransacaoRepository transacaoRepository, ICategoriaRepository categoriaRepository,
        IValidator<CreateTransacaoRequest> createValidator, IValidator<UpdateTransacaoRequest> updateValidator)
    {
        _transacaoRepository = transacaoRepository;
        _categoriaRepository = categoriaRepository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<IEnumerable<TransacaoResponseDto>> GetAllTransacoesAsync(DateTime? dataInicio, DateTime? dataFim,
        int? categoriaId, TipoCategoria? tipoCategoria)
    {
        var transacoes =
            await _transacaoRepository.GetAllTransacoesAsync(dataInicio, dataFim, categoriaId, tipoCategoria);
        return TransacaoMapper.TransacoesToTransacaoResponses(transacoes);
    }

    public async Task<TransacaoResponseDto?> GetTransacaoByIdAsync(int id)
    {
        var transacao = await _transacaoRepository.GetTransacaoByIdAsync(id);
        return transacao != null ? TransacaoMapper.MapTransacaoToResponse(transacao) : null;
    }

    public async Task<Result<TransacaoResponseDto>> CreateTransacaoAsync(CreateTransacaoRequest request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return Result<TransacaoResponseDto>.Failure(errors);
        }

        var categoria = await _categoriaRepository.GetCategoriaByIdAsync(request.CategoriaId);
        if (categoria == null || !categoria.Ativo)
        {
            return Result<TransacaoResponseDto>.Failure("A categoria especificada não existe ou não está ativa.");
        }

        var transacao = TransacaoMapper.CreateTransacaoRequestToTransacao(request);
        transacao.Data = request.Data.ToUniversalTime();
        transacao.DataCriacao = DateTime.UtcNow;

        var createdResult = await _transacaoRepository.CreateTransacaoAsync(transacao);
        if (!createdResult.IsSuccess)
        {
            return Result<TransacaoResponseDto>.Failure(createdResult.Error);
        }

        // Para aparecer corretamente a categoria
        createdResult.Value!.Categoria = categoria;
        var dto = TransacaoMapper.MapTransacaoToResponse(createdResult.Value!);
        
        return Result<TransacaoResponseDto>.Success(dto);
    }

    public async Task<Result> UpdateTransacaoAsync(UpdateTransacaoRequest request)
    {
        var validationResult = await _updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return Result.Failure(errors);
        }

        var transacaoExistente = await _transacaoRepository.GetTransacaoByIdAsync(request.Id);
        if (transacaoExistente == null)
        {
            return Result.Failure("Transação não encontrada.");
        }

        var categoriaExists = await _categoriaRepository.GetAllCategorias()
            .ContinueWith(t => t.Result.Any(c => c.Id == request.CategoriaId && c.Ativo));
        if (!categoriaExists)
        {
            return Result.Failure("A categoria especificada não existe ou não está ativa.");
        }

        var transacao = TransacaoMapper.UpdateTransacaoRequestToTransacao(request);
        transacao.Data = request.Data.ToUniversalTime(); // Debug
        transacao.DataCriacao = transacaoExistente.DataCriacao.ToUniversalTime();

        return await _transacaoRepository.UpdateTransacaoAsync(transacao);
    }

    public async Task<Result> DeleteTransacaoAsync(int id)
    {
        return await _transacaoRepository.DeleteTransacaoAsync(id);
    }
}