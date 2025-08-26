using MgFinanceiro.Application.DTOs;
using MgFinanceiro.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MgFinanceiro.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriaController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;

    public CategoriaController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CategoriaResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int? tipoCategoria)
    {
        var categorias = await _categoriaService.GetAllCategoriasAsync(tipoCategoria);
        return Ok(categorias);
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCategoriaRequest request)
    {
        var result = await _categoriaService.CreateCategoriaAsync(request);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return CreatedAtAction(nameof(GetAll), null, request);
    }
}