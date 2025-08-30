using FluentValidation;
using MgFinanceiro.Application.DTOs.Categoria;
using MgFinanceiro.Application.Interfaces;
using MgFinanceiro.Application.Mappers;
using MgFinanceiro.Domain.Common;
using MgFinanceiro.Domain.Entities;
using MgFinanceiro.Domain.Interfaces;

namespace MgFinanceiro.Application.Services;

public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IValidator<CreateCategoriaRequest> _validator;

    public CategoriaService(
        ICategoriaRepository categoriaRepository,
        IValidator<CreateCategoriaRequest> validator)
    {
        _categoriaRepository = categoriaRepository;
        _validator = validator;
    }

    public async Task<CategoriaResponseDto?> GetCategoriaByIdAsync(int id)
    {
        var categoria = await _categoriaRepository.GetCategoriaByIdAsync(id);
        return categoria != null ? CategoriaMapper.CategoriaToCategoriaResponse(categoria) : null;
    }

    public async Task<IEnumerable<CategoriaResponseDto>> GetAllCategoriasAsync(TipoCategoria? tipoCategoria = null,
        bool? statusCategoriaAtivo = null)
    {
        var categorias = await _categoriaRepository.GetAllCategorias(tipoCategoria, statusCategoriaAtivo);
        return CategoriaMapper.CategoriasToCategoriaResponses(categorias);
    }

    public async Task<Result<CategoriaResponseDto>> CreateCategoriaAsync(CreateCategoriaRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return Result<CategoriaResponseDto>.Failure(errors);
        }

        var categoria = CategoriaMapper.CreateCategoriaRequestToCategoria(request);

        // Verifica se já não existe uma categoria com este nome e status 
        var exists = await _categoriaRepository.GetAllCategorias()
            .ContinueWith(t => t.Result.Any(c => c.Nome == categoria.Nome && c.Tipo == categoria.Tipo));
        if (exists)
        {
            return Result<CategoriaResponseDto>.Failure("Já existe uma categoria com este nome e tipo.");
        }

        // Definir propriedades padrão
        categoria.DataCriacao = DateTime.UtcNow;
        categoria.Ativo = true;

        var result = await _categoriaRepository.CreateCategoria(categoria);
        return result.IsSuccess
            ? Result<CategoriaResponseDto>.Success(CategoriaMapper.CategoriaToCategoriaResponse(result.Value!))
            : Result<CategoriaResponseDto>.Failure(result.Error);
    }

    public async Task<Result<CategoriaResponseDto>> UpdateCategoriaStatusAsync(int id,
        UpdateCategoriaStatusRequest request)
    {
        if (id != request.Id)
        {
            return Result<CategoriaResponseDto>.Failure(
                "O identificador fornecido na URL não corresponde ao especificado no corpo da requisição.");
        }

        var categoria = await _categoriaRepository.GetCategoriaByIdAsync(id);
        if (categoria == null)
        {
            return Result<CategoriaResponseDto>.Failure("Categoria não encontrada.");
        }

        categoria.Ativo = request.Ativo;

        var result = await _categoriaRepository.UpdateCategoria(categoria);
        return result.IsSuccess
            ? Result<CategoriaResponseDto>.Success(CategoriaMapper.CategoriaToCategoriaResponse(result.Value!))
            : Result<CategoriaResponseDto>.Failure(result.Error);
    }
}