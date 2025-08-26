using MgFinanceiro.Application.DTOs.Categoria;
using MgFinanceiro.Application.Interfaces;
using MgFinanceiro.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MgFinanceiro.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class CategoriaController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;

    public CategoriaController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    /// <summary>
    /// Retorna todas as categorias cadastradas.
    /// </summary>
    /// <param name="tipoCategoria">Filtra pelo tipo da categoria: 1 - Receita ou 2 - Despesa.</param>
    /// <returns>Lista de categorias.</returns>
    [HttpGet]
    [SwaggerOperation(
        Summary = "Listar categorias",
        Description =
            "Retorna todas as categorias cadastradas. Pode ser filtrado por tipo (1 - Receita ou 2 - Despesa)."
    )]
    [ProducesResponseType(typeof(IEnumerable<CategoriaResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery, SwaggerParameter("Tipo da categoria para filtro: 1 (Receita) ou 2 (Despesa).",
             Required = false)]
        TipoCategoria? tipoCategoria)
    {
        var categorias = await _categoriaService.GetAllCategoriasAsync(tipoCategoria);
        return Ok(categorias);
    }


    /// <summary>
    /// Cria uma nova categoria.
    /// </summary>
    /// <param name="request">Dados da categoria a ser criada.</param>
    /// <returns>Categoria criada.</returns>
    [HttpPost]
    [SwaggerOperation(
        Summary = "Criar categoria",
        Description = "Cria uma nova categoria com nome e tipo (Receita ou Despesa)."
    )]
    [ProducesResponseType(typeof(CreateCategoriaRequest), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody, SwaggerRequestBody("A Categoria que será criada", Required = true)]
        CreateCategoriaRequest request)
    {
        var result = await _categoriaService.CreateCategoriaAsync(request);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return CreatedAtAction(nameof(GetAll), null, request);
    }
}