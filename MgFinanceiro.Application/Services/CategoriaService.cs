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

    public async Task<IEnumerable<CategoriaResponseDto>> GetAllCategoriasAsync(TipoCategoria? tipoCategoria = null)
    {
        var categorias = await _categoriaRepository.GetAllCategorias(tipoCategoria);
        return CategoriaMapper.CategoriasToCategoriaResponses(categorias);
    }

    public async Task<Result> CreateCategoriaAsync(CreateCategoriaRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return Result.Failure(errors);
        }

        var categoria = CategoriaMapper.CreateCategoriaRequestToCategoria(request);

        // Verifica se já não existe uma categoria com este nome e status
        var exists = await _categoriaRepository.GetAllCategorias()
            .ContinueWith(t => t.Result.Any(c => c.Nome == categoria.Nome && c.Tipo == categoria.Tipo && c.Ativo));
        if (exists)
        {
            return Result.Failure("Já existe uma categoria ativa com este nome e tipo.");
        }

        // Definir propriedades padrão
        categoria.DataCriacao = DateTime.UtcNow;
        categoria.Ativo = true;

        var result = await _categoriaRepository.CreateCategoria(categoria);
        return result.IsSuccess
            ? Result.Success()
            : Result.Failure(result.Error);
    }
}