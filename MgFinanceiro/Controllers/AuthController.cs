using MgFinanceiro.Application.DTOs.Auth;
using MgFinanceiro.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MgFinanceiro.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);

        if (!result.IsSuccess)
        {
            return Unauthorized(result.Error);
        }

        return Ok(result.Value);
    }
    
    [HttpPost("registrar")]
    public async Task<ActionResult<UsuarioDto>> Register([FromBody] UsuarioRegisterRequest usuarioRegisterRequest)
    {
        var result = await _authService.RegisterAsync(usuarioRegisterRequest);
            
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return CreatedAtAction(
            nameof(Register), 
            new { id = result.Value!.Id }, 
            result.Value
        );
    }
}