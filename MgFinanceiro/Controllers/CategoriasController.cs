using MgFinanceiro.Application.DTOs.Categoria;
using MgFinanceiro.Application.Interfaces;
using MgFinanceiro.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MgFinanceiro.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;

    public CategoriasController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    /// <summary>
    /// Retorna uma Categoria pelo Id passado, caso exista.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Categoria</returns>
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Obter Categoria por Id",
        Description = "Retorna os detalhes de uma categoria específica com base no ID fornecido.")]
    [ProducesResponseType(typeof(CategoriaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromQuery, SwaggerParameter("Id da categoria a ser buscada", Required = true)]
        int id)
    {
        var categoria = await _categoriaService.GetCategoriaByIdAsync(id);
        if (categoria == null)
        {
            return NotFound();
        }

        return Ok(categoria);
    }

    /// <summary>
    /// Retorna todas as categorias cadastradas.
    /// </summary>
    /// <param name="tipoCategoria">Filtra pelo tipo da categoria: 1 - Receita ou 2 - Despesa.</param>
    /// <param name="statusCategoriaAtivo"></param>
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
        TipoCategoria? tipoCategoria,
        [FromQuery,
         SwaggerParameter(
             "Define se devem ser retornadas apenas categorias ativas (true) ou apenas inativas (false). Se não informado, retorna todas.",
             Required = false)]
        bool? statusCategoriaAtivo)
    {
        var categorias = await _categoriaService.GetAllCategoriasAsync(tipoCategoria, statusCategoriaAtivo);
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

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value);
    }

    /// <summary>
    /// Atualiza o status de uma categoria.
    /// </summary>
    /// <param name="id">Id da categoria a ser atualizada.</param>
    /// <param name="updateRequest">Corpo contendo o Id e status da categoria (ativo ou inativo).</param>
    /// <returns>Categoria atualizada.</returns>
    [HttpPatch("{id}/status")]
    [SwaggerOperation(
        Summary = "Atualizar status da categoria",
        Description = "Atualiza o status (ativo ou inativo) de uma categoria específica com base no ID fornecido."
    )]
    [ProducesResponseType(typeof(CategoriaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(
        [FromRoute, SwaggerParameter("Id da categoria a ser atualizada", Required = true)]
        int id,
        [FromBody,
         SwaggerRequestBody(
             "Objeto contendo o Id e o novo status da categoria. Utilize 'true' para ativar e 'false' para inativar.",
             Required = true)]
        UpdateCategoriaStatusRequest updateRequest)
    {
        var result = await _categoriaService.UpdateCategoriaStatusAsync(id, updateRequest);
        if (!result.IsSuccess)
        {
            return result.Error == "Categoria não encontrada." ? NotFound(result.Error) : BadRequest(result.Error);
        }

        return Ok(result.Value);
    }
}